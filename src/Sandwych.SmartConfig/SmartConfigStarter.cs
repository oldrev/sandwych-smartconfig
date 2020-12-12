using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sandwych.SmartConfig
{
    public static class SmartConfigStarter
    {
        public static async Task StartAsync<TProvider>(SmartConfigArguments args,
                                                       CancellationToken cancelToken,
                                                       Action<object, DeviceDiscoveredEventArgs> onDeviceDiscovered = null,
                                                       Action<object, SmartConfigTimerEventArgs> onElapsed = null, 
                                                       int timeoutSeconds = 100)
            where TProvider : class, ISmartConfigProvider, new()
        {
            if(args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            var provider = new TProvider();
            var ctx = provider.CreateContext();
            if (onDeviceDiscovered != null)
            {
                ctx.DeviceDiscoveredEvent += new EventHandler<DeviceDiscoveredEventArgs>(onDeviceDiscovered);
            }
            using (var job = new SmartConfigJob(TimeSpan.FromSeconds(timeoutSeconds)))
            {
                if (onElapsed != null)
                {
                    job.Elapsed += new SmartConfigTimerEventHandler(onElapsed);
                }
                await job.ExecuteAsync(ctx, args, cancelToken);
            }
        }

        public static async Task StartAsync<TProvider>(SmartConfigArguments args,
                                                       Action<object, DeviceDiscoveredEventArgs> onDeviceDiscovered = null,
                                                       Action<object, SmartConfigTimerEventArgs> onElapsed = null, 
                                                       int timeoutSeconds = 100)
            where TProvider : class, ISmartConfigProvider, new()
        {
            await StartAsync<TProvider>(args, CancellationToken.None, onDeviceDiscovered, onElapsed, timeoutSeconds);
        }
    }
}
