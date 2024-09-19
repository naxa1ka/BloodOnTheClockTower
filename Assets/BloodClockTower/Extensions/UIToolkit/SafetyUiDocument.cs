using System;
using UnityEngine.UIElements;

namespace BloodClockTower.UI
{
    public class SafetyUiDocument
    {
        private readonly Lazy<VisualElement> _root;

        public VisualElement Root => _root.Value;

        public SafetyUiDocument(UIDocument uiDocument)
        {
            _root = new Lazy<VisualElement>(() =>
            {
                if (uiDocument.rootVisualElement != null)
                    return uiDocument.rootVisualElement;
                if (uiDocument.visualTreeAsset == null)
                {
                    throw new ArgumentNullException(
                        nameof(uiDocument.visualTreeAsset),
                        "Unable to recreate empty rootVisualElement because of visualTreeAsset is empty"
                    );
                }
                uiDocument.visualTreeAsset = uiDocument.visualTreeAsset;
                if (uiDocument.rootVisualElement == null)
                    throw new InvalidOperationException(
                        "RootVisualElement is still null even after recreating"
                    );
                return uiDocument.rootVisualElement;
            });
        }

        public T Q<T>(string? name = null)
            where T : VisualElement => Root.Q<T>(name);
    }
}
