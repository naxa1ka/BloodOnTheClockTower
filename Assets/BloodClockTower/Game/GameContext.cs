using UnityEngine;
using UnityEngine.UIElements;

namespace BloodClockTower.Game
{
    public class GameContext : MonoBehaviour
    {
        [field: SerializeField]
        public UIDocument UIDocument { get; private set; } = null!;
    }
}
