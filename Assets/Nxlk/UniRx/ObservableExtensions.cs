using System;
using JetBrains.Annotations;
using UniRx;

namespace Nxlk.UniRx
{
    public static class ObservableExtensions
    {
        [MustUseReturnValue]
        public static IObservable<Unit> ToUnitObservable<T>(this IObservable<T> observable)
        {
            return observable.Select(_ => Unit.Default);
        }
    }
}
