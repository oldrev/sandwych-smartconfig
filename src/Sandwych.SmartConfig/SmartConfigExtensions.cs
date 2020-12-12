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
            this ISmartConfigJob self, SmartConfigContext context, SmartConfigArguments args)
        {
            await self.ExecuteAsync(context, args, CancellationToken.None);
        }
    }
}
