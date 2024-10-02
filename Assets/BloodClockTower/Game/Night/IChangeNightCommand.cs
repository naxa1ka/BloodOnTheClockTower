namespace BloodClockTower.Game
{
    public interface IChangeNightCommand
    {
        void Execute(INight night);
    }
}