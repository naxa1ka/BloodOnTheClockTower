using UnityEngine.UIElements;

namespace BloodClockTower.UI
{
    public static class VisualElementExtensions
    {
        public static void Show(this VisualElement visualElement) =>
            SetVisible(visualElement, true);

        public static void Hide(this VisualElement visualElement) =>
            SetVisible(visualElement, false);

        public static void SetVisible(this VisualElement visualElement, bool isVisible) =>
            visualElement.EnableInClassList("hide", !isVisible);
    }
}
