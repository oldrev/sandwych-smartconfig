using Sandwych.SmartConfig.Networking;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Sandwych.SmartConfig.Util;
using System.Timers;
using System.ComponentModel;

#if DEBUG
using System.Diagnostics;
#endif

namespace Sandwych.SmartConfig
{
    public class SmartConfigJob : ISmartConfigJob
    {
        private bool _isStarted = false;
        private readonly IDatagramBroadcaster _broadcaster = new DatagramBroadcaster();
        private readonly IDatagramReceiver _receiver = new DatagramReceiver();

        public static TimeSpan TimeInterval { get; } = TimeSpan.FromSeconds(1);

        private System.Timers.Timer _timer = new System.Timers.Timer(TimeInterval.TotalMilliseconds);

        private CancellationTokenSource _timerCts = null;

        public event SmartConfigTimerEventHandler Elapsed;

        public TimeSpan Timeout { get; }
        public TimeSpan ExecutedTime { get; private set; } = TimeSpan.Zero;
        public TimeSpan LeftTime => this.Timeout - this.ExecutedTime;

        public SmartConfigJob() : this(TimeSpan.FromSeconds(60))
        {
        }

        public SmartConfigJob(TimeSpan timeout)
        {
            this.Timeout = timeout;

            this._timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.ExecutedTime = ExecutedTime.Add(TimeSpan.FromMilliseconds(_timer.Interval));
            this.Elapsed?.Invoke(this, new SmartConfigTimerEventArgs(this.Timeout, this.ExecutedTime));
            if (this.LeftTime <= TimeSpan.Zero)
            {
                if (_timer.Enabled)
                {
                    _timer.Stop();
                }

                _timerCts?.Cancel();
            }
        }

        public async Task ExecuteAsync(SmartConfigContext context, SmartConfigArguments args, CancellationToken externalCancelToken)
        {
            if (_isStarted)
            {
                throw new InvalidOperationException("Already started");
            }

            this.ExecutedTime = TimeSpan.Zero;
            _isStarted = true;
            _timerCts = new CancellationTokenSource();
            var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(externalCancelToken, _timerCts.Token);
            try
            {
                _timer.Start();
                this.Elapsed?.Invoke(this, new SmartConfigTimerEventArgs(this.Timeout, this.ExecutedTime));

                var broadcastingTask = _broadcaster.BroadcastAsync(context, args, linkedCts.Token).CancelOnFaulted(linkedCts);
                var receivingTask = _receiver.ListenAsync(context, args.LocalAddress, linkedCts.Token).CancelOnFaulted(linkedCts);
                await Task.WhenAll(broadcastingTask, receivingTask);
            }
            catch (OperationCanceledException ocex)
            {
                if (externalCancelToken.IsCancellationRequested)
                {
                    throw ocex;
                }
            }
            catch
            {
                linkedCts.Cancel();
                throw;
            }
            finally
            {
                if (_timer.Enabled)
                {
                    _timer.Stop();
                }

                linkedCts.Dispose();

                _timerCts.Dispose();
                _timerCts = null;

                ExecutedTime = TimeSpan.Zero;
                _isStarted = false;
            }
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
                    _timer.Dispose();
                    _receiver.Dispose();
                    _broadcaster.Dispose();
                }
                _isDisposed = true;
            }
        }

        ~SmartConfigJob()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            if (_isStarted)
            {
                throw new InvalidOperationException("Already started");
            }
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }

}
