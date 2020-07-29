using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB.Bson
{
    public static class BsonCustomExtensionMethods
    {
        public static BsonDocument ToBsonDocumentIgnorNull<TNominalType>(this TNominalType obj, IBsonSerializer<TNominalType> serializer = null, Action<BsonSerializationContext.Builder> configurator = null, BsonSerializationArgs args = default)
        {
            BsonDocument bsons = new BsonDocument();

            //   if (serializer == null) serializer = BsonSerializer.LookupSerializer(obj);

            //  BsonSerializationContext.Builder builder = new BsonSerializationContext.Builder();


            return bsons;
        }

    }
}
