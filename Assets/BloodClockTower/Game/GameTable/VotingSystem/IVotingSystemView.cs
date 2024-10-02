using UnityEngine.UIElements;

namespace BloodClockTower.Game
{
    public interface IVotingSystemView
    {
        Button StartVotingButton { get; }
        Button EndVotingButton { get; }
        Button ResetInitiatorButton { get; }
        Button ResetNomineeButton { get; }
        Label StateLabel { get; }
        VisualElement StateLabelContainer { get; }
    }
}