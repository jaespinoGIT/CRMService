using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security.AntiXss;

namespace CRMService.Helpers.Serialization
{
    public class AntiXssConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var stringValue = (string)reader.Value;
            ThrowIfForbiddenInput(stringValue);
            return stringValue;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var token = JToken.FromObject(value);
            token.WriteTo(writer);
        }

        private static void ThrowIfForbiddenInput(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            var encoded = AntiXssEncoder.HtmlEncode(value, true);
            if (value != encoded)
            {
                throw new Exception("Forbidden input. The following characters are not allowed: &, <, >, \", '");
            }
        }
    }

    public class NoConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanRead => false;
        public override bool CanWrite => false;
    }
}
