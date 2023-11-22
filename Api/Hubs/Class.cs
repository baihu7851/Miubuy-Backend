namespace Api.Hubs
{
    public class Class
    {
        public int RoomId { get; set; }
        public int BuyerId { get; set; }
        public int SellerId { get; set; }
        public int Status { get; set; }
    }

    public class Chat
    {
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public int RoomId { get; set; }
        public string Message { get; set; }
        public string MsgTime { get; set; }
    }

    public class History
    {
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public int RoomId { get; set; }
    }
}