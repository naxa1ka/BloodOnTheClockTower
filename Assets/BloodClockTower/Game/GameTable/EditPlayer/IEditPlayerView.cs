using UnityEngine.UIElements;

namespace BloodClockTower.Game
{
    public interface IEditPlayerView
    {
        Button StartEditingButton { get; }
        Button EndEditingButton { get; }
        Button KillPlayerButton { get; }
        TextField NameInputField { get; }
    }
}