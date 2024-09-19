using BloodClockTower.UI;
using UnityEngine.UIElements;

public class MenuView
{
    public Button StartButton { get; }
    public IntegerField PlayersAmountInputField { get; }

    public MenuView(SafetyUiDocument uiDocument)
    {
        StartButton = uiDocument.Q<Button>("start-button");
        PlayersAmountInputField = uiDocument.Q<IntegerField>("players-amount-input-field");
    }
}
