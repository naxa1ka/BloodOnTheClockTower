using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Nxlk.Scene
{
    public class UnitySceneManager : ISceneManager
    {
        public async UniTask UnloadScene(string sceneName, UnloadSceneOptions options)
        {
            await SceneManager.UnloadSceneAsync(sceneName, options);
        }

        public async UniTask LoadScene(string sceneName, LoadSceneMode mode)
        {
            await SceneManager.LoadSceneAsync(sceneName, mode);
        }
    }
}