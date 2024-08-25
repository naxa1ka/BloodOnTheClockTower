using System.Collections.Generic;
using System.Linq;

public interface ILocalizationKey
{
    string Key { get; }
}

public class EmptyNameLocalizationKey : ILocalizationKey
{
    public string Key => "EmptyName";
}

public class LocalizationKey : ILocalizationKey
{
    public string Key { get; }

    public LocalizationKey(string key) => Key = key;
}

public interface ILocalizer { }

public interface IPlayer
{
    ILocalizationKey Name { get; }
    IRoleAssumption RoleAssumption { get; }
    bool IsAlive { get; }
    void Kill();
}

public class Player : IPlayer
{
    public ILocalizationKey Name { get; } = new EmptyNameLocalizationKey();
    public IRoleAssumption RoleAssumption { get; } = new DefaultRoleAssumption();
    public bool IsAlive { get; private set; } = true;

    public void Kill()
    {
        IsAlive = false;
    }
}

public interface IRoleAssumption
{
    IRole Role { get; }
    IAssumption Assumption { get; }
}

public class DefaultRoleAssumption : IRoleAssumption
{
    public IRole Role { get; }
    public IAssumption Assumption { get; }
}

public interface IAssumption { }

public interface IRole
{
    ILocalizationKey Name { get; }
    ILocalizationKey Description { get; }
}

public class GameSession
{
    private readonly IReadOnlyList<IPlayer> _players;

    public IReadOnlyList<IPlayer> Players => _players;

    public GameSession(int playersAmount)
    {
        _players = Enumerable.Range(0, playersAmount).Select(_ => new Player()).ToList();
    }
}

public class CompositionRoot { }
