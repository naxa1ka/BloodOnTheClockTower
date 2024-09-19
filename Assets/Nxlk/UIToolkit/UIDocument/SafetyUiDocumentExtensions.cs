using UnityEngine.UIElements;

namespace Nxlk.UIToolkit
{
    public static class SafetyUiDocumentExtensions
    {
        public static SafetyUiDocument ToSafetyUiDocument(this UIDocument uiDocument) =>
            new(uiDocument);
    }
}
