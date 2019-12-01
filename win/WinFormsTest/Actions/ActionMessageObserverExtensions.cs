using System;
using System.Reactive.Linq;
using Newtonsoft.Json.Linq;
using WinFormsTest.Actions;
using WinFormsTest.Messages;

namespace WinFormsTest.Messages
{
    public static class ActionMessageObserverExtensions
    {
        public static IObservable<AngularAction> OfActions(this IObservable<Message> messages)
        {
            if (messages == null)
            {
                throw new ArgumentNullException(nameof(messages));
            }

            return messages
                .OfType<ActionsMessage>()
                .SelectMany(p => p.Body?.Actions ?? Array.Empty<AngularAction>());
        }
    }
}
