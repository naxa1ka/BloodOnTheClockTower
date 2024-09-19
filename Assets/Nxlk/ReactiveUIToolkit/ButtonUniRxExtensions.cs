using System;
using JetBrains.Annotations;
using UniRx;
using UnityEngine.UIElements;

namespace Nxlk.ReactiveUIToolkit
{
    public static class ButtonUniRxExtensions
    {
        public static IObservable<Unit> ObserveClick(this Button button)
        {
            return Observable.FromEvent(
                action => button.clicked += action,
                action => button.clicked -= action
            );
        }

        [MustUseReturnValue]
        public static IDisposable SubscribeOnClick(this Button button, Action action)
        {
            return button.ObserveClick().Subscribe(unit => action.Invoke());
        }
    }
}