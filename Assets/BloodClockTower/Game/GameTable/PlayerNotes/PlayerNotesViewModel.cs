using System;
using Nxlk.Initialization;
using Nxlk.UniRx;
using OneOf;
using OneOf.Types;
using UniRx;

namespace BloodClockTower.Game
{
    public class PlayerNotesViewModel : DisposableObject, IInitializable, IPlayerNotesViewModel
    {
        private readonly IGame _game;
        private readonly IEditPlayerViewModel _editPlayerViewModel;
        private readonly ReactiveProperty<bool> _isVisible;
        private readonly ReactiveProperty<OneOf<string, None>> _selectedPlayerNote;
        private IDisposable _selectedPlayerNoteDisposable = Disposable.Empty;

        public IReadOnlyReactiveProperty<bool> IsVisible => _isVisible;

        public string Notes
        {
            get
            {
                return _editPlayerViewModel.SelectedPlayer.Value.Match(
                    model => _game.GetNotes(model.Player.Original),
                    none => "selected player is empty"
                );
            }
        }
        public IReadOnlyReactiveProperty<OneOf<string, None>> SelectedPlayerNote =>
            _selectedPlayerNote;

        public PlayerNotesViewModel(IGame game, IEditPlayerViewModel editPlayerViewModel)
        {
            _game = game;
            _editPlayerViewModel = editPlayerViewModel;
            _isVisible = new ReactiveProperty<bool>(false).AddTo(disposables);
            _selectedPlayerNote = new ReactiveProperty<OneOf<string, None>>(new None()).AddTo(
                disposables
            );
        }

        public void Initialize()
        {
            _editPlayerViewModel.SelectedPlayer.Subscribe(OnPlayerChanged).AddTo(disposables);
        }

        private void OnPlayerChanged(OneOf<PlayerViewModel, None> player)
        {
            _selectedPlayerNoteDisposable.Dispose();
            player.Switch(
                playerViewModel =>
                    _selectedPlayerNoteDisposable = playerViewModel.Player.Note.Subscribe(
                        note => _selectedPlayerNote.Value = note
                    ),
                none => _selectedPlayerNote.Value = none
            );
        }

        public void ChangeNote(string note)
        {
            _editPlayerViewModel.SelectedPlayer.Value.Switch(
                player => player.ChangeNote(note),
                none => throw new InvalidOperationException()
            );
        }

        public void Show() => _isVisible.Value = true;

        public void Hide() => _isVisible.Value = false;
    }
}
