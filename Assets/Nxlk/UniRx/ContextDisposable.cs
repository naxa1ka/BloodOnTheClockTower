using System;
using UniRx;

namespace Nxlk.UniRx
{
    public class ContextDisposable : IDisposable
    {
        private IDisposable _disposable = Disposable.Empty;

        public void ReAttach(IDisposable disposable)
        {
            _disposable.Dispose();
            _disposable = disposable;
        }

        public void Clear()
        {
            ReAttach(Disposable.Empty);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}