using System;
using Nxlk.Bool;
using Nxlk.UniRx;
using UniRx;
using UnityEngine;

namespace BloodClockTower.Game
{
    public class PlayerViewModel : DisposableObject
    {
        [Flags]
        public enum VoteRole
        {
            None = 0,
            Default = 1 << 0,
            Initiator = 1 << 1,
            Nominee = 1 << 2,
            Participant = 1 << 3,
        }

        private readonly ReactiveProperty<Vector3> _position;
        private readonly ReactiveProperty<float> _iconSize;
        private readonly ReactiveProperty<bool> _isSelected;
        private readonly ReactiveProperty<VoteRole> _role;
        private readonly Subject<Unit> _clicked;

        public IReadOnlyReactiveProperty<Vector3> Position => _position;
        public IReadOnlyReactiveProperty<bool> IsSelected => _isSelected;
        public IReadOnlyReactiveProperty<float> IconSize => _iconSize;
        public IReactiveProperty<VoteRole> Role => _role;
        public IReadOnlyReactiveProperty<PlayerName> Name => Player.Name;
        public IObservable<Unit> Clicked => _clicked;
        public bool IsParticipant => _role.Value.HasFlag(VoteRole.Participant);
        public IPlayer Player { get; }

        public PlayerViewModel(IPlayer player)
        {
            Player = player;
            _position = new ReactiveProperty<Vector3>(Vector3.zero).AddTo(disposables);
            _iconSize = new ReactiveProperty<float>(64).AddTo(disposables);
            _isSelected = new ReactiveProperty<bool>(false).AddTo(disposables);
            _role = new ReactiveProperty<VoteRole>(VoteRole.Default).AddTo(disposables);
            _clicked = new Subject<Unit>().AddTo(disposables);
        }

        public void ChangeName(string name) => Player.ChangeName(name);

        public void SetPosition(Vector3 position) => _position.Value = position;

        public void Click() => _clicked.OnNext(Unit.Default);

        public void Select() => _isSelected.Value = true;

        public void Deselect() => _isSelected.Value = false;

        public void SetSize(float iconSize) => _iconSize.Value = iconSize;

        public void MarkInitiator() => Mark(VoteRole.Initiator);

        public void UnmarkInitiator() => Unmark(VoteRole.Initiator);

        public void MarkNominee() => Mark(VoteRole.Nominee);

        public void UnmarkNominee() => Unmark(VoteRole.Nominee);

        public void MarkParticipant() => Mark(VoteRole.Participant);

        public void UnmarkParticipant() => Unmark(VoteRole.Participant);

        public void ClearMark() => _role.Value = VoteRole.Default;

        private void Mark(VoteRole role)
        {
            _role
                .Value.Equals(VoteRole.Default)
                .Switch(() => _role.Value = role, () => _role.Value |= role);
        }

        private void Unmark(VoteRole role)
        {
            _role.Value ^= role;
        }
    }
}
