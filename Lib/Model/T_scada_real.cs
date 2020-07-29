using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Lib.Model
{
    [BsonIgnoreExtraElements]
    public class T_scada_real
    {
        [BsonId]
        // [BsonSerializer(typeof(StringOrNumberSerializer))]
        public String _id { get; set; }

        public long time { get; set; }

        [BsonSerializer(typeof(StringOrNumberSerializer))]
        public string value { get; set; }
    }
}
