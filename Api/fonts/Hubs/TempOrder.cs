using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR.Hubs;
using Miubuy.Models;

namespace Miubuy.Hubs
{
    [HubName("tempOrder")]
    public class TempOrder : Hub
    {
        private readonly Model _db = new Model();

        public void Hello()
        {
            Clients.All.hello();
        }

        public void SendDetail(int detailId, string name, decimal price, int sendId, int recipientId)
        {
            var recipient = _db.Users.Find(recipientId); //接收方
            var sender = _db.Users.Find(sendId); //發送方
            if (recipient == null || sender == null) return;
            Clients.All.ShowOrder(detailId, name, price, sendId, recipientId);
            //if (sender.Id != recipientId) //送出判斷
            //{
            //    Clients.Client(sender.ConnectionId).remindMsg(sender.Id, sender.UserName, message, msgTime);
            //}
        }

        public void UpDataDetail()
        {
            Clients.All.hello();
        }
    }
}