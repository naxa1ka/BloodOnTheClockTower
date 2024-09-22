using System;
using JetBrains.Annotations;
using Nxlk.UIToolkit;
using Nxlk.UniRx;
using UniRx;
using UnityEngine.UIElements;

namespace Nxlk.ReactiveUIToolkit
{
    public static class VisualElementUniRxExtensions
    {
        [MustUseReturnValue]
        public static IDisposable BindToVisible(
            this VisualElement visualElement,
            IObservable<bool> source
        )
        {
            return BindToVisible(source, visualElement);
        }

        [MustUseReturnValue]
        public static IDisposable BindToVisible(
            this IObservable<bool> source,
            VisualElement visualElement
        )
        {
            return source.Subscribe(visualElement.SetVisible);
        }

        [MustUseReturnValue]
        public static IObservable<Unit> ObserveBlur(this VisualElement visualElement)
        {
            return visualElement.RegisterCallbackAsObservable<BlurEvent>().ToUnitObservable();
        }
    }
}
