using System;

namespace WinFormsTest.Messages
{
    public interface IMessageRegistry
    {
        bool Add<TItem>(string name) where TItem : Message, new();

        Type GetType(string name);
    }
}
