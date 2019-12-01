using System;

namespace WinFormsTest.Messages
{
    [MessageName(MessageName)]
    public class ShowMessage : Message
    {
        public const string MessageName = "show";

        public override string Name => MessageName;
    }
}
