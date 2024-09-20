using UnityEngine;
using UnityEngine.UIElements;

namespace BloodClockTower.Bootstrap
{
    public class BoostrapContext : MonoBehaviour
    {
        [field: SerializeField]
        public VisualTreeAsset PlayerIconView { get; private set; } = null!;
    }
}
