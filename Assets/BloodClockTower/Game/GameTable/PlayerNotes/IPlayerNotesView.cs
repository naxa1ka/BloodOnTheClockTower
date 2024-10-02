using Nxlk.UIToolkit;
using UnityEngine.UIElements;

namespace BloodClockTower.Game
{
    public interface IPlayerNotesView
    {
        VisualElement PlayerNotesContainer { get; }
        Button OpenPlayerNotesButton { get; }
        Button ClosePlayerNotesButton { get; }
        Label NoteLabel { get; }
        MultiLabel PlayerHeaderLabel { get; }
        TextField NoteInputField { get; }
    }
}