using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
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

        public void Update()
        {
            var sql = new Model();
            sql.Entry(this).State = EntityState.Modified;
            sql.SaveChanges();
        }

        public void Update(Model sql)
        {
            sql.Entry(this).State = EntityState.Modified;
            sql.SaveChanges();
        }
    }
}