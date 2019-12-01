using System;

namespace WinFormsTest.Actions
{
    [ActionType(ActionType)]
    public class PingAction : AngularAction
    {
        public const string ActionType = "[Comm] Ping";

        public override string Type => ActionType;

        public int Id { get; set; }
    }
}
