using System;
using UniRx;

namespace BloodClockTower
{
    public abstract class DisposableObject : IDisposable
    {
        protected readonly CompositeDisposable disposables = new CompositeDisposable();

        public void Dispose()
        {
            disposables.Dispose();
        }
    }
}
