using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Model
{
    public class T_scada_cleaning
    {
        [BsonId]
        public BsonObjectId _id { get; set; }

        public string id { get; set; }
        public long timetag { get; set; }

        public long endtime { get; set; }

        public long starttime { get; set; }

        public List<CleanValues> values { get; set; }
    }

    public class CleanValues
    {
        public long motime { get; set; }
        // [BsonSerializer(Type.GetType("string"))]
        [BsonSerializer(typeof(StringOrNumberSerializer))]
        public string value { get; set; }
        public long time { get; set; }

    }
}
