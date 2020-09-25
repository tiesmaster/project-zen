using System.Collections.Generic;

namespace Tiesmaster.ProjectZen.BagImporter
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<(T, int)> WithIndex<T>(this IEnumerable<T> source)
        {
            var i = 0;
            foreach (var item in source)
            {
                yield return (item, i++);
            }
        }
    }
}