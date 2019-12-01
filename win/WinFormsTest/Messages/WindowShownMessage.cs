using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsTest.Messages
{
    [MessageName(MessageName)]
    public class WindowShownMessage : Message<WindowShownBody>
    {
        public const string MessageName = "window-shown";

        public override string Name => MessageName;
    }

    public class WindowShownBody
    {
        public byte[] Hwnd { get; set; }

        public IntPtr ToIntPtr()
        {
            return Hwnd != null && Hwnd.Length >= IntPtr.Size ? MemoryMarshal.Read<IntPtr>(Hwnd) : IntPtr.Zero;
        }
    }
}
