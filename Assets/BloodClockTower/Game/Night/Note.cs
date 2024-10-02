using System;

namespace BloodClockTower.Game
{
    public interface INote
    {
        INote DeepClone();
    }

    public class Note : INote
    {
        public Note(IPlayerStatus player, string note) { }

        public INote DeepClone()
        {
            throw new Exception();
        }
    }
}
