using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace Nxlk.UIToolkit
{
    public class UIToolkitEventSystem
    {
        private readonly PanelSettings _panelSettings;

        public UIToolkitEventSystem(PanelSettings panelSettings)
        {
            _panelSettings = panelSettings;
        }

        public void SelectEventSystem()
        {
            if (EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(EventSystem.current.transform.Find(_panelSettings.name).gameObject);
        }
    }
}