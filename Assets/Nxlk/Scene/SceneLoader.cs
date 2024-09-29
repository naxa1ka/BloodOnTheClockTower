using System;
using Cysharp.Threading.Tasks;
using Nxlk.UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nxlk.Scene
{
    public sealed class SceneLoader : DisposableObject
    {
        private bool _isBusy;
        private readonly ISceneManager _sceneManager;
        private readonly ISceneContext _sceneContext;
        
        public SceneLoader(ISceneManager sceneManager, ISceneContext sceneContext)
        {
            _sceneManager = sceneManager;
            _sceneContext = sceneContext;
        }

        internal async UniTask<T> LoadSingleSceneAsync<T>(string sceneName)
            where T : Component
        {
            await LoadSingleSceneAsync(sceneName);
            return FindComponentOrThrow<T>(sceneName);
        }

        internal async UniTask UnloadSceneAsync(string sceneName)
        {
            await _sceneManager.UnloadScene(sceneName, UnloadSceneOptions.None);
        }

        private async UniTask LoadSingleSceneAsync(string sceneName)
        {
            if (_isBusy)
            {
                throw new Exception($"{this} is busy, can't load '{sceneName}'");
            }
            try
            {
                _isBusy = true;
                await _sceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            }
            finally
            {
                _isBusy = false;
            }
        }

        private T FindComponentOrThrow<T>(string sceneName)
            where T : Component
        {
            var component = _sceneContext.Find<T>();
            if (!component)
            {
                throw new Exception(
                    $"{this}: Missing component {typeof(T)} in scene '{sceneName}'"
                );
            }
            return component;
        }
    }
}
