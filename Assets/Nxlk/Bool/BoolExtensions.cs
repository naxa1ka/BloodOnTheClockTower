using System;

namespace Nxlk.Bool
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
