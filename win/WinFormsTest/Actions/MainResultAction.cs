using System;

namespace WinFormsTest.Actions
{
    [ActionType(ActionType)]
    public class MainResultAction : AngularAction
    {
        public const string ActionType = "[Main] Main Result";

        public override string Type => ActionType;

        public string Text { get; set; }
        public string Result { get; set; }
    }
}
