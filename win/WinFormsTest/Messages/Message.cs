using System;
using Newtonsoft.Json;

namespace WinFormsTest.Messages
{
    [JsonConverter(typeof(MessageJsonConverter))]
    public abstract class Message
    {
        public abstract string Name { get; }
    }
}
