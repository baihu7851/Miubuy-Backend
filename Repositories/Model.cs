using System.Data.Entity;

namespace Repositories
{
    public class Model : DbContext
    {
        // 您的內容已設定為使用應用程式組態檔 (App.config 或 Web.config)
        // 中的 'Model' 連接字串。根據預設，這個連接字串的目標是
        // 如果您的目標是其他資料庫和 (或) 提供者，請修改
        // 應用程式組態檔中的 'Model' 連接字串。
        public Model() : base("name=Model")
        {
        }

        public virtual DbSet<ChatHistory> ChatHistories { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<County> Counties { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<TempDetail> TempDetails { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomUser> RoomUsers { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<City> Cities { get; set; }

        // 針對您要包含在模型中的每種實體類型新增 DbSet。如需有關設定和使用
        // Code First 模型的詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=390109。
    }
}