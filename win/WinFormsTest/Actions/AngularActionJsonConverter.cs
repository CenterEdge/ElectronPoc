using System;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace WinFormsTest.Actions
{
    public class AngularActionJsonConverter : JsonConverter
    {
        public override bool CanWrite => false;
        public override bool CanRead => true;
        public override bool CanConvert(Type objectType) => objectType == typeof(AngularAction);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);

            if (!jObject.TryGetValue("type", out var token))
            {
                return null;
            }

            if (token.Type != JTokenType.String)
            {
                return null;
            }

            var actionType = ActionRegistry.Instance.GetType(token.Value<string>());
            if (actionType != null)
            {
                var action = (AngularAction) Activator.CreateInstance(actionType);

                serializer.Populate(jObject.CreateReader(), action);

                return action;
            }
            else
            {
                return null;
            }
        }
    }
}
