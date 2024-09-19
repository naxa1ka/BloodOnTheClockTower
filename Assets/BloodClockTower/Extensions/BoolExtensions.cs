using System;

namespace BloodClockTower
{
    public static class BoolExtensions
    {
        public static void Switch(this bool value, Action trueAction, Action falseAction)
        {
            if (value)
            {
                trueAction();
            }
            else
            {
                falseAction();
            }
        }
    }
}
