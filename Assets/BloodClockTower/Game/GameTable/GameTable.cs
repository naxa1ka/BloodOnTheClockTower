using System.Linq;
using Nxlk.LINQ;
using Nxlk.UniRx;
using UniRx;

namespace BloodClockTower.Game
{
    public class GameTable : DisposableObject
    {
        private readonly ReactiveCollection<IPlayer> _players;

        public IReadOnlyReactiveCollection<IPlayer> Players => _players;

        public GameTable(int playersAmount)
        {
            _players = CollectionExtensions.AddTo(new ReactiveCollection<IPlayer>(
                Enumerable.Range(0, playersAmount).Select(_ => new Player())
            ), disposables);
        }
    }
}
