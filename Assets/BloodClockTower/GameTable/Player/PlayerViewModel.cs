using System;
using Nxlk.Bool;
using Nxlk.LINQ;
using Nxlk.UniRx;
using UniRx;
using UnityEngine;

namespace BloodClockTower
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

        private readonly IPlayer _player;
        private readonly ReactiveProperty<Vector3> _position;
        private readonly ReactiveProperty<float> _iconSize;
        private readonly ReactiveProperty<bool> _isSelected;
        private readonly ReactiveProperty<VoteRole> _role;
        private readonly Subject<Unit> _clicked;

        public IReadOnlyReactiveProperty<Vector3> Position => _position;
        public IReadOnlyReactiveProperty<bool> IsSelected => _isSelected;
        public IReadOnlyReactiveProperty<float> IconSize => _iconSize;
        public IReactiveProperty<VoteRole> Role => _role;
        public IReadOnlyReactiveProperty<PlayerName> Name => _player.Name;
        public IObservable<Unit> Clicked => _clicked;
        public bool IsParticipant => _role.Value.HasFlag(VoteRole.Participant);

        public PlayerViewModel(IPlayer player)
        {
            _player = player;
            _position = CollectionExtensions.AddTo(new ReactiveProperty<Vector3>(Vector3.zero), disposables);
            _iconSize = CollectionExtensions.AddTo(new ReactiveProperty<float>(64), disposables);
            _isSelected = CollectionExtensions.AddTo(new ReactiveProperty<bool>(false), disposables);
            _role = CollectionExtensions.AddTo(new ReactiveProperty<VoteRole>(VoteRole.Default), disposables);
            _clicked = CollectionExtensions.AddTo(new Subject<Unit>(), disposables);
        }

        public void ChangePosition(Func<Vector3, Vector3> updatePosition) =>
            SetPosition(updatePosition.Invoke(_position.Value));

        public void SetName(string name) => _player.SetName(name);

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
