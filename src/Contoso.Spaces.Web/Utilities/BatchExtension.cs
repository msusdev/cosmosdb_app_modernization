using System.Collections.Generic;
using System.Linq;

namespace Contoso.Spaces.Web.Utilities
{
    public static class BatchExtension
    {
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int count) =>
            source.Select((item, index) => (item, index))
                .GroupBy(i => i.index / count)
                .Select(g => g.Select(i => i.item));
    }
}