using System;
using System.Runtime.InteropServices;
using PInvoke;

namespace WinFormsTest
{
    public class Job : IDisposable
    {
        private readonly Kernel32.SafeObjectHandle _handle;
        private bool _disposed;

        public unsafe Job()
        {
            _handle = Kernel32.CreateJobObject(null, null);

            var info = new Kernel32.JOBOBJECT_BASIC_LIMIT_INFORMATION
            {
                LimitFlags = Kernel32.JOB_OBJECT_LIMIT_FLAGS.JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE
            };

            var extendedInfo = new Kernel32.JOBOBJECT_EXTENDED_LIMIT_INFORMATION
            {
                BasicLimitInformation = info
            };

            var length = Marshal.SizeOf(typeof(Kernel32.JOBOBJECT_EXTENDED_LIMIT_INFORMATION));
            var extendedInfoPtr = Marshal.AllocHGlobal(length);
            Marshal.StructureToPtr(extendedInfo, extendedInfoPtr, false);

            if (!Kernel32.SetInformationJobObject(_handle, Kernel32.JOBOBJECTINFOCLASS.JobObjectExtendedLimitInformation,
                extendedInfoPtr, (uint) length))
            {
                throw new Exception(
                    $"Unable to set information. Error: {Marshal.GetLastWin32Error()}");
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _handle?.Close();
            }

            _disposed = true;
        }

        public bool AddProcess(IntPtr handle)
        {
            var processHandle = new Kernel32.SafeObjectHandle(handle, false);

            return Kernel32.AssignProcessToJobObject(_handle, processHandle);
        }
    }
}
