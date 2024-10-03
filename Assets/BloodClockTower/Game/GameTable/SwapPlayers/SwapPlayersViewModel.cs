using Nxlk.Initialization;
using Nxlk.UniRx;
using OneOf;
using OneOf.Types;
using UniRx;

namespace BloodClockTower.Game
{
    public class SwapPlayersViewModel : DisposableObject, ISwapPlayersViewModel, IInitializable
    {
        private readonly IGameTableViewModel _gameTableViewModel;
        private readonly ReactiveProperty<bool> _isEditing;

        private OneOf<PlayerViewModel, None> _firstPlayer = new None();

        public IReadOnlyReactiveProperty<bool> IsEditing => _isEditing;

        public SwapPlayersViewModel(IGameTableViewModel gameTableViewModel)
        {
            _gameTableViewModel = gameTableViewModel;
            _isEditing = new ReactiveProperty<bool>().AddTo(disposables);
        }

        public void Initialize()
        {
            _gameTableViewModel.Clicked.Subscribe(SelectPlayer).AddTo(disposables);
        }

        public void StartEditing() => _isEditing.Value = true;

        public void EndEditing()
        {
            _firstPlayer.Switch(
                player1 =>
                {
                    player1.Deselect();
                    _firstPlayer = new None();
                },
                none => { }
            );
            _isEditing.Value = false;
        }

        private void SelectPlayer(PlayerViewModel player)
        {
            if (!_isEditing.Value)
                return;

            _firstPlayer.Switch(
                player1 =>
                {
                    var player2 = player;
                    var player1Position = player1.Position.Value;
                    var player2Position = player2.Position.Value;
                    player1.SetPosition(player2Position);
                    player2.SetPosition(player1Position);
                    EndEditing();
                },
                none =>
                {
                    _firstPlayer = player;
                    player.Select();
                });
        }
    }
}
