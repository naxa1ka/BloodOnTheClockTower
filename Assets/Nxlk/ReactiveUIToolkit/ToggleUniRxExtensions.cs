using System;
using UnityEngine.UIElements;

namespace Nxlk.ReactiveUIToolkit
{
    public static class ToggleUniRxExtensions
    {
        public static IObservable<bool> ObserveValue(this Toggle toggle)
        {
            return toggle.RegisterValueChangedAsObservable();
        }

    }
}