using System;
using System.Reflection;

namespace WinFormsTest.Messages
{
    public static class MessageRegistryExtensions
    {
        public static void Add<T>(this IMessageRegistry registry)
            where T: Message, new()
        {
            foreach (var attr in typeof(T).GetCustomAttributes<MessageNameAttribute>())
            {
                registry.Add<T>(attr.Name);
            }
        }
    }
}
