using UnityEngine;
using UnityEngine.UIElements;

namespace BloodClockTower.Menu
{
    public class MenuContext : MonoBehaviour
    {
        [field: SerializeField]
        public UIDocument UIDocument { get; private set; } = null!;
    }
}
