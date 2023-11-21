using System.Data.Entity;
using Miubuy.Models;

namespace Miubuy.Utils
{
    public class Sql
    {
        public static void UpData(object saveData)
        {
            if (saveData == null || saveData.ToString() == "0") return;
            var db = new Model();
            db.Entry(saveData).State = EntityState.Modified;
        }
    }
}