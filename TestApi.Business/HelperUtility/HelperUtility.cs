using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Test.Business.DTO;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Collections;

namespace Test.Business.HelperUtility
{
    public class SingleOrArrayListConverter : Newtonsoft.Json.JsonConverter
    {
        readonly bool canWrite;
        readonly IContractResolver resolver;

        public SingleOrArrayListConverter() : this(false) { }

        public SingleOrArrayListConverter(bool canWrite) : this(canWrite, null) { }

        public SingleOrArrayListConverter(bool canWrite, IContractResolver resolver)
        {
            this.canWrite = canWrite;
            this.resolver = resolver ?? new Newtonsoft.Json.JsonSerializer().ContractResolver;
        }

        static bool CanConvert(Type objectType, IContractResolver resolver)
        {
            Type itemType;
            JsonArrayContract contract;
            return CanConvert(objectType, resolver, out itemType, out contract);
        }

        static bool CanConvert(Type objectType, IContractResolver resolver, out Type itemType, out JsonArrayContract contract)
        {
            if ((itemType = objectType.GetListItemType()) == null)
            {
                itemType = null;
                contract = null;
                return false;
            }
            if ((contract = resolver.ResolveContract(objectType) as JsonArrayContract) == null)
                return false;
            var itemContract = resolver.ResolveContract(itemType);
            if (itemContract is JsonArrayContract)
                return false;
            return true;
        }

        public override bool CanConvert(Type objectType) { return CanConvert(objectType, resolver); }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            Type itemType;
            JsonArrayContract contract;

            if (!CanConvert(objectType, serializer.ContractResolver, out itemType, out contract))
                throw new JsonSerializationException(string.Format("Invalid type for {0}: {1}", GetType(), objectType));
            if (reader.MoveToContent().TokenType == JsonToken.Null)
                return null;
            var list = (IList)(existingValue ?? contract.DefaultCreator());
            if (reader.TokenType == JsonToken.StartArray)
                serializer.Populate(reader, list);
            else
                list.Add(serializer.Deserialize(reader, itemType));
            return list;
        }

        public override bool CanWrite { get { return canWrite; } }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            var list = value as ICollection;
            if (list == null)
                throw new JsonSerializationException(string.Format("Invalid type for {0}: {1}", GetType(), value.GetType()));
            if (list.Count == 1)
            {
                foreach (var item in list)
                {
                    serializer.Serialize(writer, item);
                    break;
                }
            }
            else
            {
                writer.WriteStartArray();
                foreach (var item in list)
                    serializer.Serialize(writer, item);
                writer.WriteEndArray();
            }
        }
    }

    public static partial class JsonExtensions
    {
        public static JsonReader MoveToContent(this JsonReader reader)
        {
            while ((reader.TokenType == JsonToken.Comment || reader.TokenType == JsonToken.None) && reader.Read())
                ;
            return reader;
        }

        internal static Type GetListItemType(this Type type)
        {
            // Quick reject for performance
            if (type.IsPrimitive || type.IsArray || type == typeof(string))
                return null;
            while (type != null)
            {
                if (type.IsGenericType)
                {
                    var genType = type.GetGenericTypeDefinition();
                    if (genType == typeof(List<>))
                        return type.GetGenericArguments()[0];
                }
                type = type.BaseType;
            }
            return null;
        }
    }
    public static class HelperUtility
    {
        public static List<T> ConvertJsonToList<T>(this string path)
        {
            string json = File.ReadAllText(path);

            var settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                Converters = { new SingleOrArrayListConverter(true) },
            };
            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(json, settings) ?? new List<T>();

        }

    }
}
