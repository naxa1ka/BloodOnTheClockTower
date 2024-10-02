using UnityEngine;

namespace BloodClockTower.Bootstrap
{
    public class BoostrapMono : MonoBehaviour
    {
        private void Awake()
        {
           Application.Boot();
        }
    }
}
