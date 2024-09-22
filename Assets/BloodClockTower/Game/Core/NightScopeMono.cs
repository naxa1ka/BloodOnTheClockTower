using System;
using Nxlk.UIToolkit;
using UnityEngine;

namespace BloodClockTower.Game
{
    public class NightScopeMono : MonoBehaviour
    {
        private NightScope _scope = null!;

        public void Construct(
            ViewFactory<PlayerIconView> playerIconViewFactory,
            SafetyUiDocument safetyUiDocument,
            Game game,
            Night night, UIToolkitEventSystem uiToolkitEventSystem
        )
        {
            _scope = new NightScope(playerIconViewFactory, safetyUiDocument, game, night, uiToolkitEventSystem);
            _scope.Compose();
        }

        private void Start() => _scope.Start();

        private void OnDestroy() => _scope.Dispose();
    }
}
