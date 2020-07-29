using Lib.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Lib
{
    public class MongodbHelper
    {

        private MongoClient client;
        /// <summary>
        /// mongodb://localhost:27017
        /// </summary>
        /// <param name="url"></param>
        public MongodbHelper(string url = "mongodb://localhost:27017")
        {
            client = new MongoClient(url);
        }



        public void Test()
        {
            //IMongoDatabase database = client.GetDatabase("scadadb_history");
            //IMongoCollection<T_scada_cleaning> coll = database.GetCollection<T_scada_cleaning>("t_scada_cleaning");
            //var cc = QueryAllData<T_scada_cleaning>(coll);

            var asa = QueryAllData<T_scada_cleaning>("scadadb_history", "t_scada_cleaning");

            var asaasa = QueryAllData<T_scada_cleaning>("scadadb_history", "t_scada_cleaning", M => M.id.Equals("H_11_压力"));

            //   T_scada_real realNew = new T_scada_real { value = "测试1212", _id = "H_环境温度_XSReServioir", time = 1212121 };
            T_scada_real realNew = new T_scada_real { value = "测试1212", _id = null, time = 0 };

            UpdateData<T_scada_real>("scadadb_real", "t_scada_real", realNew, M => M._id.Equals("H_环境温度_XSReServioir"));


        }



        public void UpdateData<T>(String DbBane, String CollName, T n, Expression<Func<T, bool>> f = null)
        {
            IMongoDatabase database = client.GetDatabase(DbBane);
            IMongoCollection<T> coll = database.GetCollection<T>(CollName);

            UpdateResult result = updateData<T>(coll, f, n);
        }

        private UpdateResult updateData<T>(IMongoCollection<T> collection, Expression<Func<T, bool>> f, T n)
        {
            FilterDefinition<T> filter = f;

            //   UpdateDefinition<T> u = new BsonDocument("$set", n.ToBsonDocumentIgnorNull());

            UpdateDefinition<T> u = new BsonDocument("$set", n.ToBsonDocument());

            UpdateResult fluent = collection.UpdateOne(f, u, new UpdateOptions { IsUpsert = true });

            return fluent;
        }


        /// <summary>
        /// 查询方法
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="DbBane">Db名称</param>
        /// <param name="CollName">Collection名称</param>
        /// <param name="f">Expression 查询条件 默认是空查询全部</param>
        /// <returns></returns>
        public List<T> QueryAllData<T>(String DbBane, String CollName, Expression<Func<T, bool>> f = null)
        {
            IMongoDatabase database = client.GetDatabase(DbBane);
            IMongoCollection<T> coll = database.GetCollection<T>(CollName);

            if (f == null)
                return QueryAllData(coll);
            else return QueryData<T>(coll, f);
        }
        private List<T> QueryData<T>(IMongoCollection<T> collection, Expression<Func<T, bool>> f)
        {

            IMongoQueryable<T> fluent = collection.AsQueryable<T>().Where(f);

            var data = fluent.ToList();
            return data;
        }


        private List<T> QueryAllData<T>(IMongoCollection<T> collection)
        {

            IMongoQueryable<T> fluent = collection.AsQueryable<T>();

            var data = fluent.ToList();
            return data;
        }
    }
}
