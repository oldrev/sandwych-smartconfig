using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sandwych.SmartConfig
{
    public static class SmartConfigExtensions
    {
        public static async Task ExecuteAsync(
            this ISmartConfigJob self, SmartConfigArguments args, SmartConfigContext context)
        {
            await self.ExecuteAsync(context, args, CancellationToken.None);
        }
    }
}
