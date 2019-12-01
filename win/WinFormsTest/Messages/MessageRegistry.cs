using System;

namespace WinFormsTest.Messages
{
    public static class MessageRegistry
    {
        public static IMessageRegistry Instance { get; set; } = new DefaultMessageRegistry();
    }
}
