    using UnityEngine;
    using UnityEngine.UIElements;

    public class MenuScene : Scene<MenuScene.MonoContext>
    {
        public class MonoContext : MonoBehaviour
        {
            [field: SerializeField]
            public UIDocument MenuUIDocument { get; private set; }
        }
        
        public MenuScene() : base("MenuScene")
        {
        }
    }
