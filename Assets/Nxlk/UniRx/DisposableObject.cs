using System;
using UniRx;

namespace Nxlk.UniRx
{
    public abstract class DisposableObject : IDisposable
    {
        protected readonly CompositeDisposable disposables = new CompositeDisposable();

        public bool IsDisposed { get; private set; }
        
        public void Dispose()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().Name);
            IsDisposed = true;
            disposables.Dispose();
        }
    }
}
