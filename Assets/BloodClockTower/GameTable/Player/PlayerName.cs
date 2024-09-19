namespace BloodClockTower
{
    public record PlayerName(string Name)
    {
        public override string ToString() => Name;
    }
}