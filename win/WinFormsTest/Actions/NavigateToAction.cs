using System;
using System.Collections.Generic;

namespace WinFormsTest.Actions
{
    [ActionType(ActionType)]
    public class NavigateToAction : AngularAction
    {
        public const string ActionType = "[Main] Navigate To";

        public override string Type => ActionType;

        public IList<string> Route { get; set; }
    }
}
