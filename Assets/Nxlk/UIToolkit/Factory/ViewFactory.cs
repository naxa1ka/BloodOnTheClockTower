using System;
using UnityEngine.UIElements;

namespace Nxlk.UIToolkit
{
    public class ViewFactory<T> : IViewFactory<T>
    {
        private readonly VisualTreeAsset _visualTreeAsset;

        public ViewFactory(VisualTreeAsset visualTreeAsset)
        {
            _visualTreeAsset = visualTreeAsset;
        }

        public T Create()
        {
            var container = _visualTreeAsset.CloneTree().contentContainer;
            return (T)Activator.CreateInstance(typeof(T), container);
        }
    }
}
