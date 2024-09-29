using Cysharp.Threading.Tasks;
using UObject = UnityEngine.Object;

namespace Nxlk.Scene
{
    public interface IScene<T>
        where T : UObject
    {
        UniTask<T> Load();
        UniTask Unload();
    }
}