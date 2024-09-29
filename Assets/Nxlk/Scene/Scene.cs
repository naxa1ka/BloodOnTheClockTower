using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Nxlk.Scene
{
    public class Scene<T> : IScene<T>
        where T : Component
    {
        private readonly SceneLoader _sceneLoader;

        public string Name { get; }

        public Scene(SceneLoader sceneLoader, string name)
        {
            _sceneLoader = sceneLoader;
            Name = name;
        }

        public UniTask Unload() => _sceneLoader.UnloadSceneAsync(Name);

        public UniTask<T> Load() => _sceneLoader.LoadSingleSceneAsync<T>(Name);
    }
}