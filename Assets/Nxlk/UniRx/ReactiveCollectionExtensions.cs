using System;
using UniRx;

namespace Nxlk.UniRx
{
    public static class ReactiveCollectionExtensions
    {
        public static IObservable<int> ObserveCountChangedWithCount<T>(
            this IReadOnlyReactiveCollection<T> reactiveCollection
        )
        {
            return reactiveCollection.ObserveCountChanged().StartWith(reactiveCollection.Count);
        }

        public static IObservable<T> ObserveAddItem<T>(
            this IReadOnlyReactiveCollection<T> reactiveCollection
        )
        {
            return reactiveCollection.ObserveAdd().Select(addEvent => addEvent.Value);
        }

        public static IObservable<T> ObserveAddItemWithCollection<T>(
            this IReadOnlyReactiveCollection<T> reactiveCollection
        )
        {
            return reactiveCollection.ObserveAddItem().StartWithCollection(reactiveCollection);
        }

        public static IObservable<T> ObserveRemoveItem<T>(
            this IReadOnlyReactiveCollection<T> reactiveCollection
        )
        {
            return reactiveCollection.ObserveRemove().Select(removeEvent => removeEvent.Value);
        }

        public static IObservable<T> StartWithCollection<T>(
            this IObservable<T> observable,
            IReadOnlyReactiveCollection<T> reactiveCollection
        )
        {
            return observable.StartWith(reactiveCollection);
        }
    }
}
