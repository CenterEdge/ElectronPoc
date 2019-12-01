using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using WinFormsTest.Actions;
using WinFormsTest.Messages;

namespace WinFormsTest
{
    public class MessagePipe : IDisposable
    {
        private static volatile int _pingIndex = new Random().Next(0, 999999);

        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        private static readonly Encoding Utf8NoBomEncoding = new UTF8Encoding(false);

        private readonly NamedPipeClientStream _namedPipeClient;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public bool Connected { get; private set; } = false;

        private readonly Subject<Message> _messages = new Subject<Message>();
        public IObservable<Message> Messages => _messages;

        public MessagePipe(string pipeName)
        {
            _namedPipeClient = new NamedPipeClientStream(".", pipeName, PipeDirection.InOut,
                PipeOptions.Asynchronous, TokenImpersonationLevel.Impersonation);
        }

        public async Task Connect(CancellationToken cancellationToken)
        {
            if (!Connected)
            {
                await _namedPipeClient.ConnectAsync(cancellationToken);
                Connected = true;

#pragma warning disable 4014
                Task.Run(Reader, _cancellationTokenSource.Token);
#pragma warning restore 4014
            }
        }

        public unsafe Task SendMessage(Message message, CancellationToken cancellationToken)
        {
            var messageString = JsonConvert.SerializeObject(message, SerializerSettings);

            var messageSize = Utf8NoBomEncoding.GetMaxByteCount(messageString.Length);
            var buffer = ArrayPool<byte>.Shared.Rent(messageSize + sizeof(int));
            try
            {
                var span = buffer.AsSpan();

                int byteCount;
                fixed (byte* bytes = &MemoryMarshal.GetReference(span.Slice(sizeof(int))))
                {
                    fixed (char* chars = messageString)
                    {
                        byteCount = Utf8NoBomEncoding.GetBytes(chars, messageString.Length, bytes, buffer.Length);
                    }
                }

                MemoryMarshal.Write(span, ref byteCount);

                return _namedPipeClient.WriteAsync(buffer, 0, byteCount + sizeof(int), cancellationToken)
                    // ReSharper disable once MethodSupportsCancellation
                    .ContinueWith((task, state) => ArrayPool<byte>.Shared.Return((byte[]) state), buffer);
            }
            catch
            {
                // For exceptions, we need to handle here
                ArrayPool<byte>.Shared.Return(buffer);
                throw;
            }
        }

        private async Task Reader()
        {
            var sizeBuffer = new byte[sizeof(int)];

            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                await ReadToFill(sizeBuffer, sizeBuffer.Length);

                var packetSize = BitConverter.ToInt32(sizeBuffer, 0);

                var packet = ArrayPool<byte>.Shared.Rent(packetSize);
                try
                {
                    await ReadToFill(packet, packetSize);

                    PacketReceived(packet.AsMemory(0, packetSize));
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(packet);
                }
            }
        }

        private unsafe void PacketReceived(ReadOnlyMemory<byte> packet)
        {
            string packetString;

            fixed (byte* bytes = &MemoryMarshal.GetReference(packet.Span))
            {
                packetString = Utf8NoBomEncoding.GetString(bytes, packet.Length);
            }

            var message = JsonConvert.DeserializeObject<Message>(packetString, SerializerSettings);

            Task.Factory.StartNew(state => _messages.OnNext((Message) state), message);
        }

        private async Task ReadToFill(byte[] buffer, int length)
        {
            var totalBytesRead = 0;
            while (totalBytesRead < length)
            {
                var bytesRead =
                    await _namedPipeClient.ReadAsync(buffer, totalBytesRead, length - totalBytesRead, _cancellationTokenSource.Token);
                if (bytesRead <= 0)
                {
                    throw new Exception("End of stream");
                }

                totalBytesRead += bytesRead;
                if (totalBytesRead == length)
                {
                    return;
                }
            }
        }

        public Task PingAsync(CancellationToken cancellationToken)
        {
            var pingId = Interlocked.Increment(ref _pingIndex);
            var tcs = new TaskCompletionSource<bool>();

            _messages
                .OfActions().OfType<PongAction>()
                .Where(p => p.Id == pingId)
                .Take(1)
                .ToTask(cancellationToken)
                .ContinueWith(p => { tcs.TrySetResult(true); }, cancellationToken,
                    TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.Current);

            Task.Run(async () =>
            {
                while (!tcs.Task.IsCompleted)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    await SendMessage(ActionsMessage.Create(new PingAction { Id = pingId }), cancellationToken);

                    await Task.Delay(1000, cancellationToken);
                }
            }, cancellationToken);

            return tcs.Task;
        }

        public void Dispose()
        {
            _messages.OnCompleted();
            _cancellationTokenSource.Cancel();
            _namedPipeClient.Dispose();
        }
    }
}
