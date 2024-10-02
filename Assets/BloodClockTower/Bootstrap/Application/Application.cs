using System.Globalization;
using BloodClockTower.Game;
using BloodClockTower.Menu;
using Nxlk.Scene;
using Nxlk.UIToolkit;
using Nxlk.UniRx;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;
using VContainer.Unity;
using UApplication = UnityEngine.Application;
using UObject = UnityEngine.Object;

namespace BloodClockTower.Bootstrap
{
    public class Application : DisposableObject
    {
        private static Application? _instance;
        private static GameObject? _applicationGameObject;
        private static LifetimeScope? _applicationBaseScope;

        public static bool Exists => _instance != null;

        public static void Boot()
        {
            if (_instance == null || _instance.IsDisposed)
            {
                var prefab = Resources.Load<GameObject>("ApplicationLifetimeScope");
                if (_applicationGameObject == null)
                {
                    _applicationGameObject = UObject.Instantiate(prefab);
                    _applicationBaseScope = _applicationGameObject.AddComponent<LifetimeScope>();
                    UObject.DontDestroyOnLoad(_applicationGameObject);
                }
                _instance = new Application().AddTo(_applicationGameObject);
            }
        }

        private Application()
        {
            _instance?.Dispose();
            _instance = this;

            UApplication.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            Disposable.Create(() => _instance = null).AddTo(disposables);

            _applicationBaseScope!.CreateChild(DependencyRegistration).AddTo(disposables);
        }

        private void DependencyRegistration(IContainerBuilder builder)
        {
            builder.Register<UnitySceneContext>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<UnitySceneManager>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SceneLoader>(Lifetime.Singleton).AsSelf();
            builder.Register<MenuScene>(Lifetime.Singleton).AsSelf();
            builder.Register<GameScene>(Lifetime.Singleton).AsSelf();
            builder.Register<StartGameCommand>(Lifetime.Singleton).AsSelf();
            builder.RegisterEntryPoint<MenuEntryPoint>();
            builder
                .Register<ViewFactory<PlayerIconView>>(Lifetime.Singleton)
                .As<IViewFactory<PlayerIconView>>()
                .WithParameter(Resources.Load<VisualTreeAsset>("PlayerIconView"));
        }
    }
}
