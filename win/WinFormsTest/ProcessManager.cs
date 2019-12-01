using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WinFormsTest.Messages;

namespace WinFormsTest
{
    public class ProcessManager : IDisposable
    {
        private readonly ILogger<ProcessManager> _logger;

        private readonly BehaviorSubject<MessagePipe> _messagePipeSubject = new BehaviorSubject<MessagePipe>(null);
        public IObservable<MessagePipe> MessagePipe => _messagePipeSubject;

        private Job _job;
        private Process _process;
        private string _pipeName;
        private MessagePipe _messagePipe;

        public ProcessManager(ILogger<ProcessManager> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                cancellationToken.ThrowIfCancellationRequested();

                EnsureJob();

                _pipeName = $"advantage_{Guid.NewGuid()}";

                _process = StartProcess();
                if (_process == null)
                {
                    return;
                }

                _messagePipe = await OpenPipeAsync(cancellationToken);
                _messagePipeSubject.OnNext(_messagePipe);
            }, cancellationToken);
        }

        private Process StartProcess()
        {
            try
            {
                var directory = Path.GetFullPath(Path.Combine(
                    // ReSharper disable once AssignNullToNotNullAttribute
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    @"..\..\..\..\electron-poc"));

                var processStartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = directory,
                    FileName = Path.Combine(directory, @"node_modules\.bin\electron.cmd"),
                    Arguments = $". --serve --pipeName {_pipeName}",
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };

                var process = Process.Start(processStartInfo);
                if (process == null)
                {
                    return null;
                }

                _job.AddProcess(process.Handle);
                return process;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Starting Electron Process");
                return null;
            }
        }

        private async Task<MessagePipe> OpenPipeAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    var messagePipe = new MessagePipe(_pipeName);
                    await messagePipe.Connect(cancellationToken).ConfigureAwait(false);

                    await messagePipe.PingAsync(cancellationToken);

                    return messagePipe;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    await Task.Delay(1000, cancellationToken).ConfigureAwait(false);
                }
            }
        }

        private void EnsureJob()
        {
            if (_job == null)
            {
                _job = new Job();
            }
        }

        public void Dispose()
        {
            _messagePipeSubject?.Dispose();
            _messagePipe.Dispose();
            _job?.Dispose();
        }
    }
}
