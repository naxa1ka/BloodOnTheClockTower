using System;
using UniRx;

namespace Nxlk.UniRx
{
    public static class UnitObservableExtensions
    {
        public static IDisposable Subscribe(this IObservable<Unit> observable, Action onNext)
        {
            return observable.Subscribe(unit => onNext.Invoke());
        }
    }
}
