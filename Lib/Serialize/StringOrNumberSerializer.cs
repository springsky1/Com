using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Lib
{
    public class StringOrNumberSerializer : IBsonSerializer
    {
        public Type ValueType => typeof(String);

        public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {

            switch (context.Reader.CurrentBsonType)
            {

                case BsonType.Double: { return context.Reader.ReadDouble().ToString(); }
                case BsonType.Int32: { return context.Reader.ReadInt32().ToString(); }
                case BsonType.Decimal128: { return context.Reader.ReadDecimal128().ToString(); }
                case BsonType.Int64: { return context.Reader.ReadInt64().ToString(); }
                case BsonType.String: { return context.Reader.ReadString(); }
                default: { return context.Reader.ReadString(); }
            }

            //  throw new NotImplementedException();

        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {

            // throw new NotImplementedException();
            context.Writer.WriteString(value as string);
        }
    }
}
