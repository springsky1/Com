using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lib
{
    public class RedisHelper
    {
        ConnectionMultiplexer _conn;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host">127.0.0.1:6379</param>
        public RedisHelper(String host)
        {
            _conn = ConnectionMultiplexer.Connect($"{host},allowAdmin=true");
        }

        public static void Test()
        {
            ConnectionMultiplexer _conn = ConnectionMultiplexer.Connect("127.0.0.1:6379,allowAdmin=true");
            var database = _conn.GetDatabase(0);//指定连接的库 0



            //  long lenght = database.ListRightPush("ListTest", "1212");
            bool Exist = database.KeyExists("ListTest");


            // database.list


            TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks);
            //var cc = timeSpan.TotalMilliseconds;
            //for (int i = 0; i < 1000000; i++)
            //{
            //    long lenght = database.ListRightPush("ListTest", i);
            //}
            var span = timeSpan.Subtract(new TimeSpan(DateTime.Now.Ticks)).TotalMilliseconds;

            Console.WriteLine(span);
        }
    }
}
