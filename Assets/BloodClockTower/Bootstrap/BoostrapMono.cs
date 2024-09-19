using BloodClockTower;
using UnityEngine;

public class BoostrapMono : MonoBehaviour
{
    [SerializeField] private BoostrapContext _context = null!; 
    
    private Bootstrap _bootstrap = null!;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _bootstrap = new Bootstrap(_context);
        _bootstrap.Compose();
    }

    private void Start() => _bootstrap.Start();

    private void OnDestroy() => _bootstrap.Dispose();
}