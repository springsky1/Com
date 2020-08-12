using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.DB
{
    public class DBHelper
    {
        public static void Test()
        {
            SqlSugarClient client = new SqlSugarClient(new ConnectionConfig
            {
                ConnectionString = ""

            });
        }

    }
}
