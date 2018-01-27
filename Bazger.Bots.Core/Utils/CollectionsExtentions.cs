using System;
using System.Collections.Generic;

namespace Bazger.Bots.Core.Utils
{
    public static class CollectionsExtentions
    {
        public static void AddRange<T>(this ICollection<T> target, IEnumerable<T> source)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            if (source == null)
                throw new ArgumentNullException("source");
            foreach (var element in source)
                target.Add(element);
        }
    }
}
