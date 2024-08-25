    using UnityEngine.UIElements;

    public static class SafetyUiDocumentExtensions
    {
        public static SafetyUiDocument ToSafetyUiDocument(this UIDocument uiDocument) => new(uiDocument);
    }