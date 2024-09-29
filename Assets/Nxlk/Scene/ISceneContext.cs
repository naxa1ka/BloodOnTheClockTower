using UObject = UnityEngine.Object;

namespace Nxlk.Scene
{
    public interface ISceneContext
    {
        T Find<T>() where T : UObject;
    }
}