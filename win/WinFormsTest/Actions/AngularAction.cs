using System;
using Newtonsoft.Json;

namespace WinFormsTest.Actions
{
    [JsonConverter(typeof(AngularActionJsonConverter))]
    public abstract class AngularAction
    {
        public abstract string Type { get; }
    }
}
