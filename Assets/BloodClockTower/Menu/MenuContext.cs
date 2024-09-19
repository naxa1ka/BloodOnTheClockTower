using UnityEngine;
using UnityEngine.UIElements;

public class MenuContext : MonoBehaviour
{
    [field: SerializeField]
    public UIDocument UIDocument { get; private set; } = null!;
}