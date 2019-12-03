using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using PInvoke;
using WinFormsTest.Actions;
using WinFormsTest.Messages;

// ReSharper disable IdentifierTypo

namespace WinFormsTest
{
    public class ModalForm
    {
        private readonly MessagePipe _pipe;
        private IntPtr _hwnd = IntPtr.Zero;

        public ModalForm(MessagePipe pipe)
        {
            _pipe = pipe ?? throw new ArgumentNullException(nameof(pipe));
        }

        public async Task<TResult> ShowDialog<TResult>(Form owner, params string[] route)
            where TResult: AngularAction
        {
            if (owner == null)
            {
                throw new ArgumentNullException(nameof(owner));
            }

            var tcs = new TaskCompletionSource<TResult>();

            var subscriptions = new List<IDisposable>();
            try
            {
                subscriptions.Add(_pipe.Messages
                    .OfType<WindowShownMessage>()
                    .Subscribe(p => { _hwnd = p.Body?.ToIntPtr() ?? IntPtr.Zero; }));

                subscriptions.Add(_pipe.Messages
                    .OfActions()
                    .OfType<TResult>()
                    .Subscribe(p => { tcs.TrySetResult(p); }));

                RegisterHandlers(owner);
                try
                {
                    await _pipe.SendMessage(new ActionsMessage
                    {
                        Body = new ActionsMessageBody
                        {
                            Actions = new List<AngularAction>
                            {
                                new NavigateToAction { Route = route }
                            }
                        }
                    }, CancellationToken.None);

                    await _pipe.SendMessage(new ShowMessage(), CancellationToken.None);

                    var filter = new SimpleFilter(owner.Handle);
                    Application.AddMessageFilter(filter);
                    try
                    {
                        var result = await tcs.Task;

                        await _pipe.SendMessage(new HideMessage(), CancellationToken.None);

                        return result;
                    }
                    finally
                    {
                        _hwnd = IntPtr.Zero;
                        Application.RemoveMessageFilter(filter);
                    }
                }
                finally
                {
                    UnregisterHandlers(owner);
                }
            }
            finally
            {
                foreach (var subscription in subscriptions)
                {
                    subscription.Dispose();
                }
            }
        }

        private void RegisterHandlers(Form owner)
        {
            owner.Activated += form_Activated;
        }

        private void UnregisterHandlers(Form owner)
        {
            owner.Activated -= form_Activated;
        }

        private void form_Activated(object sender, EventArgs e)
        {
            if (_hwnd != IntPtr.Zero)
            {
                User32.SetForegroundWindow(_hwnd);
            }
        }

        private class SimpleFilter : IMessageFilter
        {
            private static readonly HashSet<User32.WindowMessage> FilteredEvents = new HashSet<User32.WindowMessage>
            {
                User32.WindowMessage.WM_NCLBUTTONDOWN,
                User32.WindowMessage.WM_NCLBUTTONUP,
                User32.WindowMessage.WM_NCLBUTTONDBLCLK,
                User32.WindowMessage.WM_NCMBUTTONDOWN,
                User32.WindowMessage.WM_NCMBUTTONUP,
                User32.WindowMessage.WM_NCMBUTTONDBLCLK,
                User32.WindowMessage.WM_NCRBUTTONDOWN,
                User32.WindowMessage.WM_NCRBUTTONUP,
                User32.WindowMessage.WM_NCRBUTTONDBLCLK
            };

            private readonly IntPtr _hwnd;

            public SimpleFilter(IntPtr hwnd)
            {
                _hwnd = hwnd;
            }

            public bool PreFilterMessage(ref System.Windows.Forms.Message m)
            {
                if (m.HWnd != _hwnd)
                {
                    return false;
                }

                return FilteredEvents.Contains((User32.WindowMessage) m.Msg);
            }
        }
    }
}
