using System;

namespace WinFormsTest.Messages
{
    [MessageName(MessageName)]
    public class HideMessage : Message
    {
        public const string MessageName = "hide";

        public override string Name => MessageName;
    }
}
