using System;
using UnityEngine.UIElements;

namespace Nxlk.ReactiveUIToolkit
{
    public static class TextFieldUniRxExtensions
    {
        public static IObservable<string> ObserveText(this TextField textField)
        {
            return textField.RegisterValueChangedAsObservable();
        }
    }
}
