using UnityEngine.UIElements;

namespace BloodClockTower.Game
{
    public interface INightChangingView
    {
        Button NextNightButton { get; }
        Button PreviousNightButton { get; }
        Label NightCountLabel { get; }
    }
}