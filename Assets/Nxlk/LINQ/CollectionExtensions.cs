using System.Collections.Generic;
using System.Linq;

namespace Nxlk.LINQ
{
    public static class CollectionExtensions
    {
        public static TObj AddTo<TObj, TItem>(this TObj self, ICollection<TItem> collection)
            where TObj : TItem
        {
            collection.Add(self);
            return self;
        }

        public static IEnumerable<T> Yield<T>(this T self)
        {
            yield return self;
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> enumerable, T item)
        {
            return enumerable.Concat(item.Yield());
        }
        
        public static IEnumerable<T> Except<T>(this IEnumerable<T> enumerable, T item)
        {
            return enumerable.Except(item.Yield());
        }
    }
}
