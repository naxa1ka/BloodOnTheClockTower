using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Nxlk.Scene
{
    public interface ISceneManager
    {
        UniTask UnloadScene(string sceneName, UnloadSceneOptions options);
        UniTask LoadScene(string sceneName,  LoadSceneMode mode);
    }
}