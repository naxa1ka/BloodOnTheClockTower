using Nxlk.UniRx;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace BloodClockTower.Game
{
    public class StartGameCommand : DisposableObject
    {
        private readonly LifetimeScope _lifetimeScope;
        private readonly ContextDisposable _contextDisposable;

        public StartGameCommand(LifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
            _contextDisposable = new ContextDisposable().AddTo(disposables);
        }

        public void Execute(int playersAmount)
        {
            _lifetimeScope
                .CreateChild(builder =>
                {
                    builder.RegisterInstance(GamePlayersAmount.From(playersAmount));
                    builder.RegisterEntryPoint<GameEntryPoint>();
                })
                .ReAttach(_contextDisposable)
                .name = "StartGameCommandLifetimeScope";
            ;
        }
    }
}
