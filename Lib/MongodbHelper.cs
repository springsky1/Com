using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    public class MongodbHelper
    {

        MongoClient client { get; }
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
            IMongoDatabase database = client.GetDatabase("scadadb_history");
            IMongoCollection<object> coll = database.GetCollection<object>("t_scada_cleaning");


            var cc = QueryAllData(coll);
        }


        public void GetColl(String DbBane, String CollName)
        {
            IMongoDatabase database = client.GetDatabase(DbBane);
            IMongoCollection<object> coll = database.GetCollection<object>(CollName);
        }

        private String QueryAllData(IMongoCollection<object> collection)
        {

            IMongoQueryable<Object> fluent = collection.AsQueryable<object>();

            var data = fluent.ToList().ToJson();
            return data;
        }
    }

    public class CollQuery
    {
        private MongoClient client { get; }
        private IMongoCollection<object> coll { get; set; }
        public CollQuery(String DbBane, String CollName)
        {
            IMongoDatabase database = client.GetDatabase(DbBane);
            coll = database.GetCollection<object>(CollName);
        }


        public string QueryData(String CollName)
        {

        }

        public String QueryAll()
        {

            IMongoQueryable<Object> fluent = coll.AsQueryable<object>();

            var data = fluent.ToList().ToJson();
            return data;
        }
    }

}
