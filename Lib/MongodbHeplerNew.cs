using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;



namespace ZhiHuHot
{
    public class MongodbHeplerNew
    {
        protected static MongoClient client;
        protected IMongoDatabase db;
        static MongodbHeplerNew()
        {
            client = new MongoClient("mongodb://localhost:27017");
        }
        /// <summary>
        /// 参考地址   mongodb://localhost:27017
        /// </summary>
        /// <param name="url"></param>
        public MongodbHeplerNew(String url)
        {
            client = new MongoClient(url);
        }
        public static IMongoDatabase GetDatabase(String dbName)
        {
            return (client.GetDatabase(dbName));
        }

        public List<T> QueryPage<T>(PageInfo pageInfo, string collName, IMongoDatabase database)
        {
            IMongoCollection<T> collection = database.GetCollection<T>(collName);
            var data = collection.AsQueryable<T>();

            pageInfo.Totoal = data.Count();
            List<T> listData = new List<T>();
            if (pageInfo.IsAll)
            {
                listData = data.ToList();
            }
            else
            {
                listData = data.Skip(pageInfo.PageSize * (pageInfo.PageIndex - 1)).Take(pageInfo.PageSize).ToList();
            }
            return listData;
        }
        public IMongoQueryable<T> QueryPage<T>(string collName, IMongoDatabase database)
        {
            IMongoCollection<T> collection = database.GetCollection<T>(collName);
            var data = collection.AsQueryable<T>();


            return data;
        }

        public void InSertDocs<T>(List<T> docs, string collName, IMongoDatabase database)
        {
            IMongoCollection<T> collection = database.GetCollection<T>(collName);
            collection.InsertMany(docs);

        }




        public void Update<T>(UpdateDefinition<T> Newdocs, FilterDefinition<T> filter, string collName, IMongoDatabase database)
        {
            IMongoCollection<T> collection = database.GetCollection<T>(collName);

            //  FilterDefinition<T> filter = Builders<T>.Filter.Eq<T>(a=>a.ToString(),Olddocs);



            //  UpdateDefinition update = "";
            collection.UpdateMany(filter, Newdocs, new UpdateOptions { IsUpsert = true });

        }


        public void UpdateNew<T>(T NewDoc, T OldDoc, String collName, IMongoDatabase database)
        {
            //  var updateOpton = Builders<HotInfo>.Update.Set("", ""); 

            var filter = new BsonDocumentFilterDefinition<T>(OldDoc.ToBsonDocument());

            var newData = new BsonDocumentUpdateDefinition<T>(NewDoc.ToBsonDocument());


            //  var filter = BuildFilterOption<T>(OldDoc);
            var newDataQuery = BuildQueryOption<T>(NewDoc);
            IMongoCollection<T> collection = database.GetCollection<T>(collName);

            collection.UpdateOne(filter, newDataQuery, new UpdateOptions { IsUpsert = true });
        }



        private UpdateDefinition<T> BuildQueryOption<T>(T doc)
        {
            var update = Builders<T>.Update;
            var updates = new List<UpdateDefinition<T>>();

            var t = doc.GetType();
            var proper = t.GetProperties();
            foreach (PropertyInfo info in proper)
            {
                var value = info.GetValue(doc);
                if (value != null)
                {
                    updates.Add(update.Set(info.Name, info.GetValue(doc)));
                }

            }
            // update.Combine(updates);
            return update.Combine(updates);
        }

    }
    public class PageInfo
    {
        public bool IsAll { get; set; } = false;
        public int PageSize { get; set; } = 100;
        public int PageIndex { get; set; } = 1;
        public long Totoal { get; set; }
        public BsonDocument Query { get; set; }
    }



    public class MyMongoDB<T>
    {
        protected static MongoClient client;

        public MyMongoDB()
        {
            client = new MongoClient("mongodb://localhost:27017");
        }
        static MyMongoDB()
        {
            client = new MongoClient("mongodb://localhost:27017");
        }
        public MyCollection<T> GetCollection(string collName, string dbName)
        {
            MyCollection<T> mycollection = new MyCollection<T>();
            IMongoDatabase database = client.GetDatabase(dbName);
            IMongoCollection<T> collection = database.GetCollection<T>(collName);
            mycollection.collection = collection;
            return mycollection;
        }
    }

    public class MyCollection<T>
    {
        public IMongoCollection<T> collection;

        public List<T> QueryData()
        {
            var list = collection.AsQueryable<T>().ToList();
            return list;
        }
        public List<T> QueryData(Expression<Func<T, bool>> expression)
        {
            var list = collection.AsQueryable().Where(expression);

            //var aa = list.ToJson();
            //var cc = list.ToString();
            //var dd = list.ToList<T>();
            return list.ToList<T>();
        }
        public List<T> QueryData(PageInfo pageInfo)
        {
            List<T> list = null;
            if (pageInfo == null || pageInfo.IsAll == true)
                list = collection.AsQueryable<T>().ToList();
            else
            {
                list = collection.AsQueryable<T>().Skip(pageInfo.PageSize * (pageInfo.PageIndex - 1)).Take(pageInfo.PageSize).ToList();
            }
            return list;
        }

        public void AddDoc(T ts)
        {
            collection.InsertOne(ts);
        }

        public void AddDocs(List<T> ts)
        {
            collection.InsertMany(ts);
        }

        public void UpdateDoc(Expression<Func<T, bool>> filter, T t)
        {
            // FilterDefinition<T> filter = null;
            //  UpdateDefinition<T> update = Builders<T>.Update.ToBsonDocument();//   null;// Builders<T>.Update.
            var newData = BuildQueryOption(t);
            UpdateResult result = collection.UpdateOne(filter, newData);
        }
        //public async Task<long> Detele<T>(string collName, Expression<Func<T, bool>> predicate)
        //{
        //    var coll = db.GetCollection<T>(collName);
        //    var result = await coll.DeleteManyAsync(predicate).ConfigureAwait(false);
        //    return result.DeletedCount;
        //}
        public void Detele(Expression<Func<T, bool>> predicate)
        {
            var result = collection.DeleteMany(predicate);//.ConfigureAwait(false);
                                                          // return result.DeletedCount;
        }
        private UpdateDefinition<T> BuildQueryOption(T doc)
        {
            var update = Builders<T>.Update;
            var updates = new List<UpdateDefinition<T>>();

            var t = doc.GetType();
            var proper = t.GetProperties();
            foreach (PropertyInfo info in proper)
            {
                var value = info.GetValue(doc);
                if (value != null)
                {
                    updates.Add(update.Set(info.Name, info.GetValue(doc)));
                }

            }
            // update.Combine(updates);
            return update.Combine(updates);
        }
    }


    //public class MyMongDb
    //{

    //    public static IMongoDatabase db;      

    //    public MyMongDb(IMongoDatabase database)
    //    {
    //        db = database;
    //    }
    //    public MyCollection GetMongoDataBase(string CollName)
    //    {

    //        return new MyCollection(db.GetCollection<BsonDocument>(CollName));
    //    }




    //}
    //public class MyCollection
    //{
    //    public static IMongoCollection<BsonDocument> Collection;
    //    public MyCollection(IMongoCollection<BsonDocument> collection)
    //    {
    //        Collection = collection;
    //    }

    //    public static List<T>  GetDocument<T>(PageInfo  pageinfo) {

    //        //PageInfo pageInfo = new PageInfo() { PageIndex = 0, PageSize = 100 };
    //        // var data = Collection.Find<BsonDocument>(pageinfo.Query);
    //        //pageinfo.Totoal = data.CountDocuments();

    //        //List<T> list = new List<T>();
    //        //  IMongoDatabase db;
    //        IMongoCollection<T> collection;

    //        Collection =
    //        var data = collection.AsQueryable<T>();

    //        return null;
    //    }

    //    public static List<T> GetDocument<T>(BsonDocument query) where T : new()
    //    {

    //        List<T> ts = new List<T>();
    //        var ccc = (Collection.Find<BsonDocument>(query).ToList()).ToJson();
    //        ts = BsonSerializer.Deserialize<List<T>>(ccc.ToString());
    //        return ts;
    //    }
    //    public static void AddDocument(BsonDocument query)
    //    {

    //        Collection.InsertOne(query);
    //    }

    //    public static void AddDocuments(List<BsonDocument> query)
    //    {
    //        Collection.InsertMany(query.AsEnumerable());
    //    }
    //    public static void AddDocuments<T>(List<T> query) where T : new()
    //    {

    //        BsonArray bsonArray = new BsonArray();
    //        foreach (T t in query)
    //        {
    //            bsonArray.Add(t.ToJson());
    //        }

    //        // Collection.InsertMany(bsonArray.AsQueryable().AsEnumerable());
    //    }

    //}


}
