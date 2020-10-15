using System;
using System.Collections.Generic;
using System.Linq;

namespace Hestia.Model.Extensions
{
    public static class LinqExt
    {
        public static IEnumerable<T> DistinctBy<T, Tr>(this IEnumerable<T> enumerable, Func<T, Tr> selector) =>
            enumerable.GroupBy(selector)
                      .Select(x => x.First());
    }
}
