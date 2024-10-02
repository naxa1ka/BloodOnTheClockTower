using Nxlk.UIToolkit;
using Nxlk.UniRx;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace BloodClockTower.Game
{
    public class ChangeNightCommand : DisposableObject, IChangeNightCommand
    {
        private readonly LifetimeScope _lifetimeScope;
        private readonly GameUiDocument _gameUiDocument;
        private readonly ContextDisposable _contextDisposable;

        public ChangeNightCommand(LifetimeScope lifetimeScope, GameUiDocument gameUiDocument)
        {
            _lifetimeScope = lifetimeScope;
            _gameUiDocument = gameUiDocument;
            _contextDisposable = new ContextDisposable().AddTo(disposables);
        }

        public void Execute(Night night)
        {
            _lifetimeScope
                .CreateChild(builder =>
                {
                    builder.RegisterInstance(night);

                    builder
                        .Register<GameTableViewModel>(Lifetime.Singleton)
                        .AsImplementedInterfaces()
                        .AsSelf();
                    builder
                        .Register<VotingHistoryViewModel>(Lifetime.Singleton)
                        .AsImplementedInterfaces()
                        .AsSelf();
                    builder
                        .Register<VotingSystemViewModel>(Lifetime.Singleton)
                        .AsImplementedInterfaces()
                        .AsSelf();
                    builder
                        .Register<EditPlayerViewModel>(Lifetime.Singleton)
                        .AsImplementedInterfaces()
                        .AsSelf();

                    builder
                        .Register<GameTableView>(Lifetime.Singleton)
                        .AsSelf()
                        .WithParameter<ISafetyUiDocument>(_gameUiDocument);
                    builder
                        .Register<GameTablePresenter>(Lifetime.Singleton)
                        .AsImplementedInterfaces();

                    builder
                        .Register<EditPlayerView>(Lifetime.Singleton)
                        .AsSelf()
                        .WithParameter<ISafetyUiDocument>(_gameUiDocument);

                    builder
                        .Register<EditPlayerPresenter>(Lifetime.Singleton)
                        .AsImplementedInterfaces();

                    builder
                        .Register<NightChangingView>(Lifetime.Singleton)
                        .AsSelf()
                        .WithParameter<ISafetyUiDocument>(_gameUiDocument);
                    builder
                        .Register<NightChangingPresenter>(Lifetime.Singleton)
                        .AsImplementedInterfaces();

                    builder
                        .Register<VotingHistoryView>(Lifetime.Singleton)
                        .AsSelf()
                        .WithParameter<ISafetyUiDocument>(_gameUiDocument);
                    builder
                        .Register<VotingHistoryPresenter>(Lifetime.Singleton)
                        .AsImplementedInterfaces();

                    builder
                        .Register<VotingSystemView>(Lifetime.Singleton)
                        .AsSelf()
                        .WithParameter<ISafetyUiDocument>(_gameUiDocument);
                    builder
                        .Register<VotingSystemPresenter>(Lifetime.Singleton)
                        .AsImplementedInterfaces();
                })
                .ReAttach(_contextDisposable)
                .name = $"Night{night.Number}LifetimeScope";
        }
    }
}
