using Sandwych.SmartConfig.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Sandwych.SmartConfig.Networking
{
    public class DatagramBroadcaster : IDatagramBroadcaster
    {
        private IDatagramClient _broadcastingSocket;
        private bool _isStarted = false;
        private IPEndPoint _broadcastTarget;

        public DatagramBroadcaster()
        {
            _broadcastingSocket = new DefaultDatagramClient();
        }

        public DatagramBroadcaster(IDatagramClient client)
        {
            _broadcastingSocket = client;
        }

        public async Task BroadcastAsync(SmartConfigContext context, SmartConfigArguments args, CancellationToken cancelToken)
        {
            if (_isStarted)
            {
                throw new InvalidOperationException("Already started");
            }

            try
            {
                _broadcastTarget = new IPEndPoint(IPAddress.Broadcast, context.GetOption<int>(StandardOptionNames.BroadcastingTargetPort));
                var encoder = context.Provider.CreateProcedureEncoder();
                var segments = encoder.Encode(context, args);
                var broadcastBuffer = this.CreateBroadcastBuffer(segments.SelectMany(x => x.Frames));

                await this.BroadcastProcedureAsync(context, segments, broadcastBuffer, cancelToken);
            }
            finally
            {
                _isStarted = false;
            }
        }

        private async Task BroadcastProcedureAsync(
            SmartConfigContext context,
            IEnumerable<Segment> segments,
            byte[] broadcastBuffer,
            CancellationToken userCancelToken)
        {
            var segmentInterval = context.GetOption<TimeSpan>(StandardOptionNames.SegmentInterval);
            while (true)
            {
                userCancelToken.ThrowIfCancellationRequested();

                foreach (var segment in segments)
                {
                    userCancelToken.ThrowIfCancellationRequested();

                    if (segment.BroadcastingMaxTimes > 0)
                    {
                        await this.BroadcastSegmentByTimesAsync(context, segment, broadcastBuffer, userCancelToken);
                    }
                    else
                    {
                        var timerTask = Task.Delay(segment.BroadcastingPeriod, userCancelToken);
                        using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(userCancelToken))
                        {
                            linkedCts.CancelAfter(segment.BroadcastingPeriod);
                            try
                            {
                                await this.BroadcastSegmentForeverAsync(context, segment, broadcastBuffer, linkedCts.Token);
                            }
                            catch (OperationCanceledException ocex)
                            {
                                if (userCancelToken.IsCancellationRequested)
                                {
                                    throw ocex;
                                }
                            }
                        }
                    }
                    await Task.Delay(segmentInterval, userCancelToken);
                }

                await Task.Delay(segmentInterval, userCancelToken);
            }
        }

        private async Task BroadcastSegmentForeverAsync(
            SmartConfigContext context, Segment segment, byte[] broadcastBuffer, CancellationToken token)
        {
            var segmentInterval = context.GetOption<TimeSpan>(StandardOptionNames.SegmentInterval);
            while (true)
            {
                await this.BroadcastSingleSegmentAsync(segment, broadcastBuffer, segmentInterval, token);
            }
        }

        private async Task BroadcastSegmentByTimesAsync(
            SmartConfigContext context, Segment segment, byte[] broadcastBuffer, CancellationToken token)
        {
            var segmentInterval = context.GetOption<TimeSpan>(StandardOptionNames.FrameInterval);
            for (int i = 0; i < segment.BroadcastingMaxTimes; i++)
            {
                await this.BroadcastSingleSegmentAsync(segment, broadcastBuffer, segmentInterval, token);
            }
        }

        private async Task BroadcastSingleSegmentAsync(
            Segment segment, byte[] broadcastBuffer, TimeSpan segmentInterval, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            foreach (var frame in segment.Frames)
            {
                token.ThrowIfCancellationRequested();
                await _broadcastingSocket.SendAsync(broadcastBuffer, frame, this._broadcastTarget);
                if (segment.FrameInterval > TimeSpan.Zero)
                {
                    await Task.Delay(segment.FrameInterval, token);
                }
            }
            if (segmentInterval > TimeSpan.Zero)
            {
                await Task.Delay(segmentInterval, token);
            }
        }

        public byte[] CreateBroadcastBuffer(IEnumerable<ushort> frames)
        {
            var maxLength = frames.Max();
            var bytes = new byte[maxLength];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)'1';
            }
            return bytes.ToArray();
        }

        #region IDisposable Support
        private bool _isDisposed = false; // To detect redundant calls

        public void Close()
        {
            this.Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _broadcastingSocket.Dispose();
                }
                _isDisposed = true;
            }
        }

        ~DatagramBroadcaster()
        {
            this.Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            if (_isStarted)
            {
                throw new InvalidOperationException("Already started.");
            }
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
