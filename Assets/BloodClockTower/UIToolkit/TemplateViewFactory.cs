using UnityEngine.UIElements;

namespace BloodClockTower.UI
{
    public class TemplateViewFactory
    {
        private readonly VisualTreeAsset _visualTreeAsset;

        public TemplateViewFactory(VisualTreeAsset visualTreeAsset)
        {
            _visualTreeAsset = visualTreeAsset;
        }

        public VisualElement Create()
        {
             return _visualTreeAsset.CloneTree().contentContainer;
        }
    }
}