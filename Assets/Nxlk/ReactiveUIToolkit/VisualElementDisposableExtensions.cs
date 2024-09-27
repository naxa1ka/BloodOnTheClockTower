using System;
using JetBrains.Annotations;
using UniRx;
using UnityEngine.UIElements;

namespace Nxlk.ReactiveUIToolkit
{
    public static class VisualElementDisposableExtensions
    {
        [MustUseReturnValue]
        public static IDisposable RemoveFromHierarchyAsDisposable(this VisualElement visualElement)
        {
         return   Disposable.Create(() => visualElement.RemoveFromHierarchy());
        }
    }
}