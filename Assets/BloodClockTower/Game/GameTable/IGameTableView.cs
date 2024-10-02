using UnityEngine.UIElements;

namespace BloodClockTower.Game
{
    public interface IGameTableView
    {
        VisualElement Root { get; }
        VisualElement Board { get; }
    }
}