using System;
using UniRx;
using UnityEngine;

namespace Nxlk.ReactiveUIToolkit
{
    public class EscapeObservable : IObservable<Unit>
    {
        public IDisposable Subscribe(IObserver<Unit> observer)
        {
            return Observable
                .EveryUpdate()
                .Where(_ => Input.GetKey(KeyCode.Escape))
                .Subscribe(_ => observer.OnNext(Unit.Default));
        }
    }
}
