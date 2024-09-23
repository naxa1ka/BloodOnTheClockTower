using System;
using Nxlk.UniRx;
using UniRx;
using UnityEngine;

namespace Nxlk.ReactiveUIToolkit
{
    public class EscapeObservable : IObservable<Unit>
    {
        // ReSharper disable once InconsistentNaming
        private static readonly Lazy<IObservable<Unit>> instance =
            new(
                () =>
                    Observable
                        .EveryUpdate()
                        .Where(_ => Input.GetKey(KeyCode.Escape))
                        .ToUnitObservable()
            );

        public static IObservable<Unit> Instance => instance.Value;

        public IDisposable Subscribe(IObserver<Unit> observer) => Instance.Subscribe(observer);
    }
}
