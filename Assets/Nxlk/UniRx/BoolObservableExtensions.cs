using System;
using UniRx;

namespace Nxlk.UniRx
{
    public static class BoolObservableExtensions
    {
        public static IObservable<Unit> WhereTrue(this IObservable<bool> source) =>
            source.Where(x => x).Select(_ => Unit.Default);

        public static IObservable<bool> InverseBool(this IObservable<bool> source) =>
            source.Select(x => !x);
    }
}
