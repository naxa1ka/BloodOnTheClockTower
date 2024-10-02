using System;

namespace Nxlk.VContainer
{
    public class Factory<TParam1, TValue> : IFactory<TParam1, TValue>
    {
        private readonly Func<TParam1, TValue> _factory;

        public Factory(Func<TParam1, TValue> factory)
        {
            _factory = factory;
        }

        public TValue Create(TParam1 param)
        {
            return _factory(param);
        }
    }
}
