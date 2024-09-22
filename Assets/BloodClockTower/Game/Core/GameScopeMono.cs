using Nxlk.UIToolkit;
using UnityEngine;

namespace BloodClockTower.Game
{
    public class GameScopeMono : MonoBehaviour
    {
        private GameScope _gameScope = null!;

        public void Construct(
            ViewFactory<PlayerIconView> playerIconViewFactory,
            SafetyUiDocument safetyUiDocument,
            int playersAmount
        )
        {
            _gameScope = new GameScope(playerIconViewFactory, safetyUiDocument, playersAmount);
        }

        private void Start() => _gameScope.Initialize();

        private void OnDestroy() => _gameScope.Dispose();
    }
}
