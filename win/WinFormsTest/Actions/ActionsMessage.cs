using System;
using System.Collections.Generic;
using WinFormsTest.Messages;

namespace WinFormsTest.Actions
{
    [MessageName(MessageName)]
    public class ActionsMessage : Message<ActionsMessageBody>
    {
        public const string MessageName = "actions";

        public override string Name => MessageName;

        public static ActionsMessage Create(AngularAction action) =>
            new ActionsMessage
            {
                Body = new ActionsMessageBody
                {
                    Actions = new List<AngularAction>(1)
                    {
                        action
                    }
                }
            };

        public static ActionsMessage Create(params AngularAction[] actions) =>
            new ActionsMessage
            {
                Body = new ActionsMessageBody
                {
                    Actions = new List<AngularAction>(actions)
                }
            };

        public static ActionsMessage Create(IEnumerable<AngularAction> actions) =>
            new ActionsMessage
            {
                Body = new ActionsMessageBody
                {
                    Actions = new List<AngularAction>(actions)
                }
            };
    }

    public class ActionsMessageBody
    {
        public IList<AngularAction> Actions { get; set; }
    }
}
