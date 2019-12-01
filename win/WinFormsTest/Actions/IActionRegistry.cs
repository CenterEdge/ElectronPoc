using System;

namespace WinFormsTest.Actions
{
    public interface IActionRegistry
    {
        bool Add<TItem>(string type) where TItem : AngularAction, new();

        Type GetType(string type);
    }
}
