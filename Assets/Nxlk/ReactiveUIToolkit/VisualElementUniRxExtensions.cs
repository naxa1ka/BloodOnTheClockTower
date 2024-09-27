using System;
using JetBrains.Annotations;
using Nxlk.UIToolkit;
using Nxlk.UniRx;
using UniRx;
using UnityEngine;
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
        public static IDisposable BindToImageTintColor(
            this IObservable<Color> source,
            VisualElement visualElement
        )
        {
            return source.Subscribe(
                color => visualElement.style.unityBackgroundImageTintColor = new StyleColor(color)
            );
        }  
        
        [MustUseReturnValue]
        public static IDisposable BindToTransformScale(
            this IObservable<Vector3> source,
            VisualElement visualElement
        )
        {
            return source.Subscribe(
                newScale => visualElement.transform.scale = newScale
            );
        }   
        
        [MustUseReturnValue]
        public static IDisposable BindToTransformPosition(
            this IObservable<Vector3> source,
            VisualElement visualElement
        )
        {
            return source.Subscribe(
                newScale => visualElement.transform.position = newScale
            );
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
