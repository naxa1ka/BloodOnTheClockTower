using Nxlk.UIToolkit;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

namespace BloodClockTower.Game
{
    public class GameTableView
    {
        public VisualElement Root { get; private set; }
        public VisualElement Board { get; private set; }

        public GameTableView(SafetyUiDocument safetyUiDocument)
        {
            Root = safetyUiDocument.Q<VisualElement>("root");
            Board = safetyUiDocument.Q<VisualElement>("board");
        }
    }
}
