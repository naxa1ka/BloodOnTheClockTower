using BloodClockTower.UI;
using UnityEngine.UIElements;

namespace BloodClockTower
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