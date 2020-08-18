using Lib;
using Lib.DB;
using Lib.Npoi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {


            DBHelper dBHelper = new DBHelper("");

            List<UserInfo> users = dBHelper.GetData<UserInfo>(" select * from T_user ");

            // DBHelper.Test();

            //    RedisHelper.Test();

            // MongodbHelper mongodb = new MongodbHelper();
            // mongodb.Test();

            //   NPOIHelper.Test();

            //  ExcelTestClass.ImportExcelTest();

            //ExcelTestClass.temp();

            // testc();
        }

        static void testc()
        {



            String asas = Regex.IsMatch(("12120001000.1212120000001000000\u0000\u0000").Replace("\u0000", ""), "^[0-9|.]*$") ? ("12120001000.1212120000001000000\u0000\u0000").Replace("\u0000", "").TrimEnd('.', '0') : ("12120001000.1212120000001000000\u0000\u0000").Replace("\u0000", "");
            //  String asasasas = ("true000\u0000\u0000").Replace("\u0000", "").TrimEnd('.', '0');
            String asasasas = Regex.IsMatch(("true000\u0000\u0000").Replace("\u0000", ""), "^[0-9|.]*$") ? ("true000\u0000\u0000").Replace("\u0000", "").TrimEnd('.', '0') : ("true000\u0000\u0000").Replace("\u0000", "");

        }
    }
}
