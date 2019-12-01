using System;

namespace WinFormsTest.Actions
{
    public static class ActionRegistry
    {
        public static IActionRegistry Instance { get; set; } = new DefaultActionRegistry();
    }
}
