using Nxlk.UIToolkit;
using UnityEngine.UIElements;

namespace BloodClockTower.Menu
{
    public class MenuView : IMenuView
    {
        public Button StartButton { get; }
        public IntegerField PlayersAmountInputField { get; }

        public MenuView(ISafetyUiDocument uiDocument)
        {
            StartButton = uiDocument.Q<Button>("start-button");
            PlayersAmountInputField = uiDocument.Q<IntegerField>("players-amount-input-field");
        }
    }
}
