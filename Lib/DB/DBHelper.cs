using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbType = SqlSugar.DbType;

namespace Lib.DB
{
    public class DBHelper
    {
        private SqlSugarClient db;


        public DBHelper(String connnStrName)
        {
            db = new SqlSugarClient(
                   new ConnectionConfig()
                   {
                       ConnectionString = "Data Source=LocalService;User Id=C##root;Password=123456;",
                       DbType = DbType.Oracle,//设置数据库类型
                       IsAutoCloseConnection = true,//自动释放数据务，如果存在事务，在事务结束后释放
                       InitKeyType = InitKeyType.Attribute //从实体特性中读取主键自增列信息
                   });
        }

        public static void Test()
        {
            //LocalService   C##root  123456 
            SqlSugarClient db = new SqlSugarClient(
                     new ConnectionConfig()
                     {
                         ConnectionString = "Data Source=LocalService;User Id=C##root;Password=123456;",
                         DbType = DbType.Oracle,//设置数据库类型
                         IsAutoCloseConnection = true,//自动释放数据务，如果存在事务，在事务结束后释放
                         InitKeyType = InitKeyType.Attribute //从实体特性中读取主键自增列信息
                     });

            var data = db.Ado.GetDataTable("select * from T_user");
        }


        public DataTable GetDataTable(String sql)
        {
            return db.Ado.GetDataTable(sql);
        }

        public List<T> GetData<T>(String sql)
        {
            List<T> list = db.Queryable<T>().AS("T_user").ToList();

            return list;
        }

    }

    // [SugarTable("T_user")]
    public class UserInfo
    {
        public string ID { get; set; }
        public string USERID { get; set; }
    }
}
