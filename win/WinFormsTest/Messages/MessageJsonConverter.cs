using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace WinFormsTest.Messages
{
    public class MessageJsonConverter : JsonConverter
    {
        public override bool CanWrite => false;
        public override bool CanRead => true;
        public override bool CanConvert(Type objectType) => objectType == typeof(Message);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);

            if (!jObject.TryGetValue("name", out var token))
            {
                return null;
            }

            if (token.Type != JTokenType.String)
            {
                return null;
            }

            var messageType = MessageRegistry.Instance.GetType(token.Value<string>());
            if (messageType != null)
            {
                var message = (Message) Activator.CreateInstance(messageType);

                serializer.Populate(jObject.CreateReader(), message);

                return message;
            }
            else
            {
                return null;
            }
        }
    }
}
