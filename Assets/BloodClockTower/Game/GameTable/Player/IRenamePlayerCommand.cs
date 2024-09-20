namespace BloodClockTower.Game
{
    public interface IRenamePlayerCommand
    {
        public void Execute(IPlayer player, string newName) { }
    }
}
