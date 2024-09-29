using UObject = UnityEngine.Object;

namespace Nxlk.Scene
{
    public class UnitySceneContext : ISceneContext
    {
        public T Find<T>() where T : UObject
        {
            return UObject.FindObjectOfType<T>();
        }
    }
}