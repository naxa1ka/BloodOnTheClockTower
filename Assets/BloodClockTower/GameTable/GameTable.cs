using System.Linq;
using BloodClockTower;
using UniRx;

public class GameTable : DisposableObject
{
    private readonly ReactiveCollection<IPlayer> _players;

    public IReadOnlyReactiveCollection<IPlayer> Players => _players;

    public GameTable(int playersAmount)
    {
        _players = new ReactiveCollection<IPlayer>(
            Enumerable.Range(0, playersAmount).Select(_ => new Player())
        ).AddTo(disposables);
    }
}
