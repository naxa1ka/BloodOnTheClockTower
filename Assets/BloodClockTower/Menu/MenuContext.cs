using UnityEngine;

namespace BloodClockTower.Menu
{
    public class MenuContext : MonoBehaviour
    {
        [field: SerializeField]
        public MenuUiDocument UIDocument { get; private set; } = null!;
    }
}
