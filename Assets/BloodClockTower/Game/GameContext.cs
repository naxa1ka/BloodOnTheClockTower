using UnityEngine;
using UnityEngine.UIElements;

namespace BloodClockTower.Game
{
    public class GameContext : MonoBehaviour
    {
        [field: SerializeField]
        public UIDocument UIDocument { get; private set; } = null!;

        [field: SerializeField]
        public PanelSettings PanelSettings { get; private set; } = null!;
    }
}
