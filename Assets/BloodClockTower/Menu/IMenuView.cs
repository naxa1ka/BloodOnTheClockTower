using UnityEngine.UIElements;

namespace BloodClockTower.Menu
{
    public interface IMenuView
    {
        Button StartButton { get; }
        IntegerField PlayersAmountInputField { get; }
    }
}
