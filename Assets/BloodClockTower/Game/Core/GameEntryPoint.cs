using System.Threading;
using Cysharp.Threading.Tasks;
using Nxlk.UniRx;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace BloodClockTower.Game
{
    public class GameEntryPoint : DisposableObject, IAsyncStartable
    {
        private readonly GameScene _gameScene;
        private readonly LifetimeScope _lifetimeScope;

        public GameEntryPoint(GameScene gameScene, LifetimeScope lifetimeScope)
        {
            _gameScene = gameScene;
            _lifetimeScope = lifetimeScope;
        }

        public async UniTask StartAsync(CancellationToken cancellation = default)
        {
            var gameContext = await _gameScene.Load();
            _lifetimeScope
                .CreateChild(builder => DependencyRegistration(builder, gameContext))
                .AddTo(disposables)
                .name = "GameEntryPointLifetimeScope";
        }

        private void DependencyRegistration(IContainerBuilder builder, GameContext gameContext)
        {
            builder.RegisterInstance(gameContext.UIDocument);
            builder.Register<ChangeNightCommand>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<Game>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }
    }
}
