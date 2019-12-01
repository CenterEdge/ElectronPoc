using System;

namespace WinFormsTest.Actions
{
    [ActionType(ActionType)]
    public class PongAction: AngularAction
    {
        public const string ActionType = "[Comm] Pong";

        public override string Type => ActionType;

        public int Id { get; set; }
    }
}
