namespace BloodClockTower.Game
{
    public record PlayerName(string Name)
    {
        public override string ToString() => Name;
    }
}
