using Nxlk.UIToolkit;
using UnityEngine.UIElements;

namespace BloodClockTower.Game
{
    public class InteractionModeView
    {
        public Toggle EditToggle { get; set; }

        public InteractionModeView(SafetyUiDocument safetyUiDocument)
        {
            EditToggle = safetyUiDocument.Q<Toggle>("edit-toggle");
        }
    }
}
