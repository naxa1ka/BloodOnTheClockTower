using System;
using JetBrains.Annotations;
using UniRx;
using UnityEngine.UIElements;

namespace Nxlk.UIToolkit
{
    public static class ManipulatorExtensions
    {
        [MustUseReturnValue]
        public static IDisposable AddManipulatorWithDispose(
            this VisualElement visualElement,
            IManipulator manipulator
        )
        {
            visualElement.AddManipulator(manipulator);
            return Disposable.Create(() => visualElement.RemoveManipulator(manipulator));
        }
    }
}
