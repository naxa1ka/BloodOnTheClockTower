using UnityEngine;
using UnityEngine.UIElements;

namespace BloodClockTower
{
    public class BoostrapContext : MonoBehaviour
    {
        [field: SerializeField]
        public VisualTreeAsset PlayerIconView { get; private set; } = null!;

        [field: SerializeField]
        public CoroutineRunner CoroutineRunner { get; private set; } = null!;
    }
}
