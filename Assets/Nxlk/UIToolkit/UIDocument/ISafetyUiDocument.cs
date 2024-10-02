using UnityEngine.UIElements;

namespace Nxlk.UIToolkit
{
    public interface ISafetyUiDocument
    {
        T Q<T>(string? name = null)
            where T : VisualElement;
    }
}