using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sandwych.SmartConfig
{
    public interface ISmartConfigJob : IDisposable
    {
        TimeSpan Timeout { get; }

        Task ExecuteAsync(
            SmartConfigContext context,
            SmartConfigArguments args,
            CancellationToken cancelToken);
    }
}
