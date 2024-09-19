using System;
using UniRx;

namespace BloodClockTower
{
    public static class UniRxExtensions
    {
        public static IDisposable Subscribe(this IObservable<Unit> observable, Action onNext)
        {
            return observable.Subscribe(unit => onNext.Invoke());
        }

        public static IObservable<bool> InverseBool(this IObservable<bool> source) =>
            source.Select(x => !x);
    }
}
