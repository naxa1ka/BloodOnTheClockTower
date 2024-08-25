using System.Collections.Generic;

    public static class CollectionExtensions
    {
        public static TObj AddTo<TObj,TItem>(this TObj self, ICollection<TItem> collection) where TObj : TItem
        {
            collection.Add(self);
            return self;
        }
    }
