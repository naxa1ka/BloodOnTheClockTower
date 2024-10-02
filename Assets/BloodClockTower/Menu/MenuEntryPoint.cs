using System.Threading;
using Cysharp.Threading.Tasks;
using Nxlk.UIToolkit;
using Nxlk.UniRx;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace BloodClockTower.Menu
{
    public class MenuEntryPoint : DisposableObject, IAsyncStartable
    {
        private readonly LifetimeScope _menuScope;
        private readonly MenuScene _menuScene;

        public MenuEntryPoint(LifetimeScope menuScope, MenuScene menuScene)
        {
            _menuScope = menuScope;
            _menuScene = menuScene;
        }

        public async UniTask StartAsync(CancellationToken cancellation = default)
        {
            var menuContext = await _menuScene.Load();
            _menuScope
                .CreateChild(builder => DependencyRegistration(builder, menuContext))
                .AddTo(disposables)
                .name = "MenuEntryPointLifetimeScope";
        }

        private void DependencyRegistration(IContainerBuilder builder, MenuContext menuContext)
        {
            builder
                .Register<MenuView>(Lifetime.Singleton)
                .AsImplementedInterfaces()
                .WithParameter<ISafetyUiDocument>(menuContext.UIDocument);
            builder.Register<MenuPresenter>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}
