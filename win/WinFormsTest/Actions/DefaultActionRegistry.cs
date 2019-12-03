using System;
using System.Collections.Concurrent;
using WinFormsTest.Helpers;

namespace WinFormsTest.Actions
{
    public class DefaultActionRegistry : TypeRegistryBase<AngularAction>, IActionRegistry
    {
        private readonly ConcurrentDictionary<string, Type> _actions = new ConcurrentDictionary<string, Type>();

        public DefaultActionRegistry()
        {
            AddDefaultActions(this);
        }

        public static void AddDefaultActions(IActionRegistry registry)
        {
            registry.Add<DialogResultAction>();
            registry.Add<NavigateToAction>();
            registry.Add<PingAction>();
            registry.Add<PongAction>();
            registry.Add<MainResultAction>();
        }
    }
}
