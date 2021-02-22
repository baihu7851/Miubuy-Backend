using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Miubuy.Models;
using System.Data.Entity;
using Miubuy.Utils;

namespace Miubuy.Hubs
{
    [HubName("chatHub")]
    public class ChatHub : Hub
    {
        private readonly Model _db = new Model();

        #region 使用者連接

        public override Task OnConnected()
        {
            AddUserGroup();
            UpdateRoomList();
            return base.OnConnected();
        }

        #endregion 使用者連接

        #region 公開

        public void PublicSendMsg(int senderId, string message)
        {
            var msgTime = DateTime.Now;
            var sender = _db.Users.Find(senderId);
            if (sender == null) return;
            Clients.All.publicSendMsg(senderId, sender.Nickname, message, msgTime.ToString("t"));
            AddChatHistory(ChatType.公開聊天, senderId, 0, 0, message, msgTime);
        }

        #endregion 公開

        #region 私聊

        public void PrivateSendMsg(int senderId, int recipientId, string message)
        {
            var msgTime = DateTime.Now;
            var sender = _db.Users.Find(senderId);
            var recipient = _db.Users.Find(recipientId);
            if (recipient == null || sender == null) return;
            Clients.Caller.privateSendMsg(senderId, recipientId, message, msgTime.ToString("t"));
            AddChatHistory(ChatType.私人聊天, senderId, recipientId, 0, message, msgTime);
        }

        #endregion 私聊

        #region 群聊設定

        #region 創建群組

        // 用不到
        public void CreateRoom(string roomName)
        {
            var room = new Room()
            {
                Name = roomName,
            };
            _db.Rooms.Add(room);
            _db.SaveChanges();
            Clients.Client(Context.ConnectionId).showGroupMsg("success");
        }

        #endregion 創建群組

        #region 加入群組

        public void JoinRoom(int userId, int roomId)
        {
            var room = _db.Rooms.Find(roomId);
            var user = _db.Users.Find(userId);
            // 確認是否在房間
            var inRoom = room.RoomUsers.FirstOrDefault(x => x.User.Id == userId);
            if (inRoom != null) return;
            {
                // 不再則進入房間
                var roomUser = new RoomUser()
                {
                    UserId = userId
                };
                _db.RoomUsers.Add(roomUser);
                //使用者訊息增加房間
                Groups.Add(Context.ConnectionId, roomId.ToString()); //增加到群組
                Clients.Group(roomId.ToString()).groupSendMsg(userId);
            }
        }

        #endregion 加入群組

        #region 群聊

        public void GroupSendMsg(int senderId, int roomId, string message)
        {
            var msgTime = DateTime.Now;
            var room = _db.Rooms.Find(roomId);
            var sender = _db.Users.Find(senderId);
            if (room == null || sender == null) return;
            Clients.Group(roomId.ToString()).groupSendMsg(senderId, roomId, message, msgTime.ToString("t"));
            AddChatHistory(ChatType.群組聊天, senderId, 0, roomId, message, msgTime);
        }

        #endregion 群聊

        #region 退出群組

        public void LeaveRoom(int userId, int roomId)
        {
            var room = _db.Rooms.Find(roomId);

            var user = _db.RoomUsers.FirstOrDefault(x => x.UserId == userId);
            _db.RoomUsers.Remove(user);
            // 房間沒人刪除房間
            if (!room.RoomUsers.Select(x => x.RoomId == roomId).Any())
            {
                _db.Rooms.Remove(room);
            }
            Groups.Remove(Context.ConnectionId, roomId.ToString());
            UpdateRoomList();
            //更新房间列表
            Clients.Client(Context.ConnectionId).leaveRoom();
        }

        #endregion 退出群組

        #endregion 群聊設定

        #region 取得歷史紀錄

        public void GetHistory(ChatType chatType, int senderId, int recipientId, int roomId, string message, DateTime msgTime)
        {
            var chatHistories = _db.ChatHistories;

            chatHistories = (DbSet<ChatHistory>)chatHistories.Where(x => x.RoomId == roomId &&
               (x.SenderId == senderId && x.RecipientId == recipientId) ||
               (x.SenderId == recipientId && x.RecipientId == senderId));
            Clients.All().getHistory(chatHistories);
        }

        #endregion 取得歷史紀錄

        #region 增加歷史訊息

        public void AddChatHistory(ChatType chatType, int senderId, int recipientId, int roomId, string message, DateTime msgTime)
        {
            var history = new ChatHistory()
            {
                ChatType = chatType,
                SenderId = senderId,
                RecipientId = recipientId,
                RoomId = roomId,
                Message = message,
                MsgTime = msgTime
            };
            _db.ChatHistories.Add(history);
            _db.SaveChanges();
        }

        #endregion 增加歷史訊息

        #region 增加連線的使用者

        public void AddUserGroup()
        {//需要改
            var user = new RoomUser()
            {
                Id = Convert.ToInt32(Context.ConnectionId)
            };
            _db.RoomUsers.Add(user);
            _db.SaveChanges();
        }

        #endregion 增加連線的使用者

        #region 更新群組列表

        public void UpdateRoomList()
        {
            var room = _db.Rooms.ToList();
            Clients.All.showRoomList(room);
        }

        #endregion 更新群組列表
    }
}