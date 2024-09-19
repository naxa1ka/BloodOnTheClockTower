using System;
using JetBrains.Annotations;
using UniRx;
using UnityEngine.UIElements;

namespace BloodClockTower.UI
{
    public static class VisualElementUniRxExtensions
    {
        public static IObservable<Unit> WhereTrue(this IObservable<bool> source)
        {
            return source.Where(x => x == true).Select(_=> Unit.Default);
        }
          
        [MustUseReturnValue]
        public static IDisposable BindToVisible(
            this VisualElement visualElement, IObservable<bool> source
            
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

        public static IObservable<bool> ObserveValue(this Toggle toggle)
        {
            return toggle.RegisterValueChangedAsObservable();
        }

        public static IObservable<string> ObserveText(this TextField textField)
        {
            return textField.RegisterValueChangedAsObservable();
        }

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
