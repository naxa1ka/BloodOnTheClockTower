using BloodClockTower.Game;

namespace BloodClockTower.Menu
{
    public class MenuViewModel
    {
        private readonly StartGameCommand _startGameCommand;

        public MenuViewModel(StartGameCommand startGameCommand)
        {
            _startGameCommand = startGameCommand;
        }

        public void StartGame(int playersAmount)
        {
            _startGameCommand.Execute(playersAmount);
        }
    }
}
