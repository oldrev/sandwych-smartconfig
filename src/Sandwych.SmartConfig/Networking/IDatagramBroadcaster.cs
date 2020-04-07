using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sandwych.SmartConfig.Networking
{
    public interface IDatagramBroadcaster : IDisposable
    {
        Task BroadcastAsync(
            SmartConfigContext context, SmartConfigArguments args, CancellationToken cancelToken);
    }
}
