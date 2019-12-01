using System;
using Newtonsoft.Json;

namespace WinFormsTest.Messages
{
    public abstract class Message<T> : Message
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public T Body { get; set; }
    }
}
