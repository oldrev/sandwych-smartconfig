//https://stackoverflow.com/questions/438188/split-a-collection-into-n-parts-with-linq
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sandwych.SmartConfig.Test.Util
{
    public static class LinqExtensions
    {
        public static IEnumerable<IEnumerable<T>> Partition<T>(
            this IEnumerable<T> source, int partitionSize)
        {
            int i = 0;
            return source.GroupBy(x => i++ / partitionSize);
        }

    }
}
