using UnityEngine;

public class BoostrapMono : MonoBehaviour
{
    private readonly Bootstrap _bootstrap = new();
    
    private void Awake() => _bootstrap.Compose();

    private void Start() => _bootstrap.Start();

    private void OnDestroy() => _bootstrap.Dispose();
}