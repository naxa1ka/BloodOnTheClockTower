using System;

namespace Nxlk.UniRx
{
    public static class ContextDisposableExtensions
    {
        public static T ReAttach<T>(this T disposable, ContextDisposable target)
            where T : IDisposable
        {
            target.ReAttach(disposable);
            return disposable;
        }
    }
}