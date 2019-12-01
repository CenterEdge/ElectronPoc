using System;
using System.Collections.Concurrent;

namespace WinFormsTest.Helpers
{
    public abstract class TypeRegistryBase<T>
        where T: class
    {
        private readonly ConcurrentDictionary<string, Type> _registry = new ConcurrentDictionary<string, Type>();

        public virtual bool Add<TItem>(string type)
            where TItem : T, new()
        {
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(type));
            }

            return _registry.TryAdd(type, typeof(TItem));
        }

        public virtual Type GetType(string type)
        {
            if (_registry.TryGetValue(type, out var registeredType))
            {
                return registeredType;
            }

            return null;
        }
    }
}
