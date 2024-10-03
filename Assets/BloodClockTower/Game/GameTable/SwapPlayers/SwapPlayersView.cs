using Nxlk.UIToolkit;
using UnityEngine.UIElements;

namespace BloodClockTower.Game
{
    public class SwapPlayersView : ISwapPlayersView
    {
        public Button SwapPlayersButton { get; }

        public SwapPlayersView(ISafetyUiDocument safetyUiDocument)
        {
            SwapPlayersButton = safetyUiDocument.Q<Button>("swap-players-button");
        }
    }
}