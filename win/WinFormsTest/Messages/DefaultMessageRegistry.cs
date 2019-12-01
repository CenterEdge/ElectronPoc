using System;
using WinFormsTest.Actions;
using WinFormsTest.Helpers;

namespace WinFormsTest.Messages
{
    public class DefaultMessageRegistry : TypeRegistryBase<Message>, IMessageRegistry
    {
        public DefaultMessageRegistry()
        {
            AddDefaultMessages(this);
        }

        public static void AddDefaultMessages(IMessageRegistry registry)
        {
            registry.Add<ShowMessage>();
            registry.Add<HideMessage>();
            registry.Add<WindowShownMessage>();
            registry.Add<ActionsMessage>();
        }
    }
}
