using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR.Hubs;
using Miubuy.Models;
using Miubuy.Utils;

namespace Miubuy.Hubs
{
    [HubName("miuHub")]
    public class MiuHub : Hub
    {
        private readonly Model _db = new Model();

        public void CheckOrder(int roomId)
        {
            var room = _db.Rooms.Find(roomId);
            if (room == null) return;
            var connectionId = Context.ConnectionId;
            var user = _db.Users.First(x => x.ConnectionId == connectionId);
            var roomUser = _db.RoomUsers.First(x => x.RoomId == roomId && x.UserId == user.Id);
            roomUser.Status = UserStatus.訂單確認;
            _db.Entry(roomUser).State = EntityState.Modified;
            _db.SaveChanges();
            Clients.Group(roomId.ToString()).GetroomUsers(new
            {
                user.Id,
                user.Picture,
                room.SellerId,
                SellerPicture = _db.Users.FirstOrDefault(x => x.Id == room.SellerId)?.Picture,
                Status = roomUser.Status.ToString()
            });
        }

        public void ReCheckOrder(int buyerId, int roomId, int orderId)
        {
            var room = _db.Rooms.Find(roomId);
            if (room == null) return;
            var user = _db.Users.Find(buyerId);
            var roomUser = _db.RoomUsers.First(x => x.RoomId == roomId && x.UserId == buyerId);
            roomUser.Status = UserStatus.訂單送出;
            _db.Entry(roomUser).State = EntityState.Modified;
            _db.SaveChanges();
            Clients.Group(roomId.ToString()).GetroomUsers(new
            {
                Id = buyerId,
                user.Picture,
                room.SellerId,
                SellerPicture = _db.Users.FirstOrDefault(x => x.Id == room.SellerId)?.Picture,
                Status = roomUser.Status.ToString(),
                OrderId = orderId
            });
        }

        public void Chked(int roomId)
        {
            var room = _db.Rooms.Find(roomId);
            var user = _db.Users.Find(room.SellerId);
            Clients.Client(user.ConnectionId).chked(true);
        }

        #region 取得房間使用者

        public void GetRoomUsers(int roomId)
        {
            var room = _db.Rooms.Find(roomId);
            if (room == null) return;
            var roomUser = room.RoomUsers.Where(x => x.RoomId == roomId);
            Clients.Group(roomId.ToString()).GetroomUsers(roomUser.Select(user => new
            {
                user.User.Id,
                user.User.Picture,
                user.Room.SellerId,
                SellerPicture = _db.Users.FirstOrDefault(x => x.Id == user.Room.SellerId)?.Picture,
                room.Orders.FirstOrDefault(x => x.BuyerId == user.User.Id && x.SellerId == room.SellerId)?.Status,
                Count = roomUser.Count()
            }).First());
        }

        #endregion 取得房間使用者

        #region 讀取品項

        public void ReadDetail(int buyerId, int roomId)
        {
            var tempDetails = _db.TempDetails.Where(x => x.RoomId == roomId && x.BuyerId == buyerId);
            Clients.Group(roomId.ToString()).detail(tempDetails.OrderByDescending(x => x.Id).Select(tempDetail => new
            {
                tempDetail.Id,
                tempDetail.Name,
                tempDetail.Price,
                tempDetail.RoomId,
                tempDetail.BuyerId
            }));
        }

        #endregion 讀取品項

        #region 新增品項

        public void NewDetail(TempDetail[] detail, int buyerId, int roomId)
        {
            var tempDetails = _db.TempDetails.Where(x => x.RoomId == roomId && x.BuyerId == buyerId);
            foreach (var tempDetail in tempDetails)
            {
                _db.TempDetails.Remove(tempDetail);
            }
            _db.SaveChanges();
            foreach (var tempDetail in detail)
            {
                tempDetail.RoomId = roomId;
                tempDetail.BuyerId = buyerId;
                _db.TempDetails.Add(tempDetail);
            }
            _db.SaveChanges();
            Clients.Group(roomId.ToString()).detail(tempDetails.OrderByDescending(x => x.Id).Select(tempDetail => new
            {
                tempDetail.Id,
                tempDetail.Name,
                tempDetail.Price,
                tempDetail.BuyerId
            }));
        }

        #endregion 新增品項

        #region 刪除品項

        public void DelDetail(int id)
        {
            var detail = _db.TempDetails.Find(id);
            var tempDetails = _db.TempDetails.Where(x => x.RoomId == detail.RoomId && x.BuyerId == detail.BuyerId);
            _db.TempDetails.Remove(detail);
            _db.SaveChanges();
            Clients.All.detail(tempDetails.OrderByDescending(x => x.Id).Select(tempDetail => new
            {
                tempDetail.Id,
                tempDetail.Name,
                tempDetail.Price,
                tempDetail.BuyerId
            }));
        }

        #endregion 刪除品項

        #region 加入房間

        public void JoinRoom(int id, int roomId)
        {
            var user = _db.Users.Find(id);
            user.ConnectionId = Context.ConnectionId;
            Groups.Add(Context.ConnectionId, roomId.ToString());
            _db.SaveChanges();
            Clients.Group(roomId.ToString()).joinRoom(id, roomId);
        }

        #endregion 加入房間

        #region 發送訊息

        public static string String2Json(string s)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < s.Length; i++)
            {
                var c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("\\\\"); break;
                    case '/':
                        sb.Append("\\/"); break;
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    case '\v':
                        sb.Append("\\v"); break;
                    case '\0':
                        sb.Append("\\0"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }

        public void SendGroup(Chat message)
        {
            //var sender = _db.Users.Find(message.SenderId);
            //var recipient = _db.Users.Find(message.RecipientId);
            //var room = _db.Users.Find(message.RoomId);
            //if (recipient == null || sender == null || room == null) return;
            message.MsgTime = DateTime.Now.ToString("t");
            message.Message = String2Json(message.Message).Trim();
            if (string.IsNullOrEmpty(message.Message)) return;
            AddChatHistory(ChatType.群組聊天, message.SenderId, message.RecipientId, message.RoomId, message.Message, DateTime.Now);
            Clients.Group(message.RoomId.ToString()).message(message);
        }

        #endregion 發送訊息

        #region 離開房間

        public void LeaveRoom(int roomId)
        {
            var user = _db.Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            Groups.Remove(Context.ConnectionId, roomId.ToString());
            Clients.Client(Context.ConnectionId).leaveRoom();
            user.ConnectionId = "";
            _db.SaveChanges();
            //Clients.Group(roomId.ToString()).log($"使用者{user.Id}離開群組{roomId}");
        }

        #endregion 離開房間

        #region 取得歷史紀錄

        public void MessageHistory(History history)
        {
            var chatHistories = _db.ChatHistories
                .Where(x => x.RoomId == history.RoomId && (
                            (x.SenderId == history.SenderId && x.RecipientId == history.RecipientId) ||
                            x.SenderId == history.RecipientId && x.RecipientId == history.SenderId)).ToList();
            Clients.Group(history.RoomId.ToString()).messageHistory(chatHistories.OrderBy(x => x.MsgTime).Select(chatHistory => new
            {
                chatHistory.SenderId,
                chatHistory.RecipientId,
                chatHistory.RoomId,
                chatHistory.Message,
                MsgTime = chatHistory.MsgTime.ToString("t")
            }));
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
    }
}