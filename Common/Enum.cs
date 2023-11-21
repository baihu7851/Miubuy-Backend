namespace Common
{
    public enum OrderStatus
    {
        未選擇 = 0,
        未付款 = 1,
        已付款 = 2,
        超時付款 = 3,
        已退款 = 4,
        備貨中 = 5,
        已發貨 = 6,
        退貨中 = 7,
        已退貨 = 8,
        訂單完成 = 9,
        訂單取消 = 10
    }

    public enum Payment
    {
        未選擇 = 0,
        平台付款 = 1,
        直接匯款 = 2,
    }

    public enum UserStatus
    {
        無訂單 = 0,
        訂單確認 = 1,
        訂單送出 = 2,
    }

    public enum Pickup
    {
        未選擇 = 0,
        面交 = 1,
        宅配 = 2,
    }

    public enum ChatType
    {
        公開聊天 = 0,
        群組聊天 = 1,
        私人聊天 = 2
    }
}