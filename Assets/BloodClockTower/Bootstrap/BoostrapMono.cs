using UnityEngine;

namespace BloodClockTower.Bootstrap
{
    public class BoostrapMono : MonoBehaviour
    {
        [SerializeField]
        private BoostrapContext _context = null!;

        private Bootstrap _bootstrap = null!;

        private async void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _bootstrap = new Bootstrap(_context);
            await _bootstrap.Load();
            _bootstrap.Compose();
        }

        private void Start() => _bootstrap.Initialize();

        private void OnDestroy() => _bootstrap.Dispose();
    }
}
