using System;
using UniRx;
using UnityEngine.UIElements;

namespace Nxlk.ReactiveUIToolkit
{
    public static class EventsUniRxExtensions
    {
        public static IObservable<T> RegisterValueChangedAsObservable<T>(
            this INotifyValueChanged<T> control
        )
        {
            return control.RegisterChangeEventAsObservable().Select(x => x.newValue);
        }

        public static IObservable<ChangeEvent<T>> RegisterChangeEventAsObservable<T>(
            this INotifyValueChanged<T> control
        )
        {
            return Observable.FromEvent<EventCallback<ChangeEvent<T>>, ChangeEvent<T>>(
                x => x.Invoke,
                x => control.RegisterValueChangedCallback(x),
                x => control.UnregisterValueChangedCallback(x)
            );
        }

        public static IObservable<TEventType> RegisterCallbackAsObservable<TEventType>(
            this VisualElement visualElement
        )
            where TEventType : EventBase<TEventType>, new()
        {
            return Observable.FromEvent<EventCallback<TEventType>, TEventType>(
                x => x.Invoke,
                x => visualElement.RegisterCallback(x),
                x => visualElement.UnregisterCallback(x)
            );
        }
    }
}