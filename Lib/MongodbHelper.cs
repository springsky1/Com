using Lib.Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.IdGenerators;
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
            //  T_scada_real realNew = new T_scada_real { value = "测试333" };

            // var realNew = new { value = "测试444", time = 121212 };
            var realNew = new { value = "测试555" };
            //   UpdateData<T_scada_real>("scadadb_real", "t_scada_real", realNew, M => M._id.Equals("H_时雨量_XSReServioir"));


            List<T_scada_real> realsNew = new List<T_scada_real>();
            for (int i = 0; i < 10; i++)
            {
                realsNew.Add(new T_scada_real { value = "测试123_" + i, time = i * 1000, _id = BsonObjectId.GenerateNewId().ToString() });
            }
            Insert<T_scada_real>("scadadb_real", "t_scada_real", realsNew, true);

            DeleteMany<T_scada_real>("scadadb_real", "t_scada_real", M => M.value.Contains("测试"));

        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="DbBane"></param>
        /// <param name="CollName"></param>
        /// <param name="ts"></param>
        /// <param name="IsOrdered"></param>
        public void Insert<T>(String DbBane, String CollName, List<T> ts, bool IsOrdered = true)
        {
            IMongoDatabase database = client.GetDatabase(DbBane);
            IMongoCollection<T> coll = database.GetCollection<T>(CollName);

            coll.InsertMany(ts, new InsertManyOptions { IsOrdered = IsOrdered });
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="DbBane"></param>
        /// <param name="CollName"></param>
        /// <param name="f"></param>
        public void DeleteMany<T>(String DbBane, String CollName, Expression<Func<T, bool>> f)
        {
            IMongoDatabase database = client.GetDatabase(DbBane);
            IMongoCollection<T> coll = database.GetCollection<T>(CollName);

            DeleteResult result = coll.DeleteMany(f);
        }



        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="DbBane">数据库名称</param>
        /// <param name="CollName">数据集名称</param>
        /// <param name="n">新的数据 直接定义一个新对象 字段没有不更新 </param>
        /// <param name="f">条件</param>
        public void UpdateData<T>(String DbBane, String CollName, object n, Expression<Func<T, bool>> f = null)
        {
            IMongoDatabase database = client.GetDatabase(DbBane);
            IMongoCollection<BsonDocument> coll = database.GetCollection<BsonDocument>(CollName);

            UpdateResult result = updateData<T>(coll, f, n);
        }

        private UpdateResult updateData<T>(IMongoCollection<BsonDocument> collection, Expression<Func<T, bool>> f, object n)
        {
            FilterDefinition<T> filter = f;

            // BsonDocument bsons = new BsonDocument("$set", n.ToBsonDocument()); 

            UpdateDefinition<BsonDocument> update = BuildUpdateDoc(n.ToBsonDocument());

            UpdateResult fluent = collection.UpdateOne(f.ToJson(), update, new UpdateOptions { IsUpsert = true });



            return fluent;// fluent;
        }

        private static UpdateDefinition<BsonDocument> BuildUpdateDoc(BsonDocument bsons)
        {
            var update = Builders<BsonDocument>.Update;
            var updates = new List<UpdateDefinition<BsonDocument>>();

            for (int i = 0; i < bsons.ElementCount; i++)
            {
                if (bsons.GetElement(i).Value != BsonNull.Value)
                {
                    updates.Add(Builders<BsonDocument>.Update.Set(bsons.GetElement(i).Name, bsons.GetElement(i).Value.ToJson()));
                }
                else if (bsons.GetElement(i).Value.IsBsonArray)
                {

                }
            }
            return update.Combine(updates);
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
