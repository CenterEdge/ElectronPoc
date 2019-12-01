using System;
using System.Reflection;

namespace WinFormsTest.Actions
{
    public static class ActionRegistryExtensions
    {
        public static void Add<T>(this IActionRegistry registry)
            where T: AngularAction, new()
        {
            foreach (var attr in typeof(T).GetCustomAttributes<ActionTypeAttribute>())
            {
                registry.Add<T>(attr.Type);
            }
        }
    }
}
