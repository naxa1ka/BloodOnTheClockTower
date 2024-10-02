using UnityEngine;

namespace BloodClockTower.Game
{
    public class GameContext : MonoBehaviour
    {
        [field: SerializeField]
        public GameUiDocument UIDocument { get; private set; } = null!;
    }
}
