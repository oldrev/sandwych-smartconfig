// https://stackoverflow.com/questions/41899842/how-to-cancel-and-raise-an-exception-on-task-whenall-if-any-exception-is-raised
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sandwych.SmartConfig.Util
{
    public static class TaskExtensions
    {
        public static Task CancelOnFaulted(this Task task, CancellationTokenSource cts)
        {
            task.ContinueWith(x => cts.Cancel(), cts.Token, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.Default);
            return task;
        }

        public static Task<T> CancelOnFaulted<T>(this Task<T> task, CancellationTokenSource cts)
        {
            task.ContinueWith(x => cts.Cancel(), cts.Token, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.Default);
            return task;
        }

    }
}
