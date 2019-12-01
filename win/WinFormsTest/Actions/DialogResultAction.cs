using System;

namespace WinFormsTest.Actions
{
    [ActionType(ActionType)]
    public class DialogResultAction : AngularAction
    {
        public const string ActionType = "[Main] Dialog Result";

        public override string Type => ActionType;

        public string Result { get; set; }
    }
}
