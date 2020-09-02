using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace ProjectZen
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