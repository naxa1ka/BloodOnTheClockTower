using Nxlk.UIToolkit;
using UnityEngine.UIElements;

namespace BloodClockTower.Game
{
    public class NightChangingView
    {
        public Button NextNightButton { get; private set; }
        public Button PreviousNightButton { get; private set; }
        public Label HeaderLabel { get; private set; }

        public NightChangingView(SafetyUiDocument safetyUiDocument)
        {
            NextNightButton = safetyUiDocument.Q<Button>("next-night-button");
            PreviousNightButton = safetyUiDocument.Q<Button>("prev-night-button");
            HeaderLabel = safetyUiDocument.Q<Label>("header-label");
        }
    }
}
