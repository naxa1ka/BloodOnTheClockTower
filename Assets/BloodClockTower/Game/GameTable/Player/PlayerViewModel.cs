using System;
using Nxlk.Bool;
using Nxlk.UniRx;
using UniRx;
using UnityEngine;

namespace BloodClockTower.Game
{
    public class PlayerViewModel : DisposableObject
    {
        private readonly ReactiveProperty<Vector3> _position;
        private readonly ReactiveProperty<float> _iconSize;
        private readonly ReactiveProperty<bool> _isSelected;
        private readonly ReactiveProperty<VoteRole> _role;
        private readonly Subject<Unit> _clicked;

        public IReadOnlyReactiveProperty<Vector3> Position => _position;
        public IReadOnlyReactiveProperty<bool> IsSelected => _isSelected;
        public IReadOnlyReactiveProperty<float> IconSize => _iconSize;
        public IReadOnlyReactiveProperty<VoteRole> Role => _role;
        public IReadOnlyReactiveProperty<PlayerName> Name => Player.Name;
        public IReadOnlyReactiveProperty<bool> IsAlive => Player.IsAlive;
        public IObservable<Unit> Clicked => _clicked;
        public bool IsParticipant => _role.Value.IsParticipant;
        public bool IsNominee => _role.Value.IsNominee;
        public bool IsInitiator => _role.Value.IsInitiator;
        public bool IsIgnoredParticipant => _role.Value.IsIgnored;
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

        public void Kill() => Player.Kill();

        public void Revive() => Player.Revive();

        public void ChangeName(string name) => Player.ChangeName(name);

        public void SetPosition(Vector3 position) => _position.Value = position;

        public void Click() => _clicked.OnNext(Unit.Default);

        public void Select() => _isSelected.Value = true;

        public void Deselect() => _isSelected.Value = false;

        public void SetSize(float iconSize) => _iconSize.Value = iconSize;

        public void MarkInitiator() => _role.Value = _role.Value.MarkInitiator;

        public void UnmarkInitiator() => _role.Value = _role.Value.UnmarkInitiator;

        public void MarkNominee() => _role.Value = _role.Value.MarkNominee;

        public void UnmarkNominee() => _role.Value = _role.Value.UnmarkNominee;

        public void MarkParticipant() => _role.Value = _role.Value.MarkParticipant;

        public void UnmarkParticipant() => _role.Value = _role.Value.UnmarkParticipant;

        public void ClearMark() => _role.Value = VoteRole.Default;
    }
}
