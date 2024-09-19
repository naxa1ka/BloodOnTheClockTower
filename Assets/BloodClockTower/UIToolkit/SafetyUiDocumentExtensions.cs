    using UnityEngine.UIElements;

    namespace BloodClockTower.UI
    {
        public static class SafetyUiDocumentExtensions
        {
            public static SafetyUiDocument ToSafetyUiDocument(this UIDocument uiDocument) => new(uiDocument);
        }
    }