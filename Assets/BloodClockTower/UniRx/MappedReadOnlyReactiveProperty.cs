using System;
using UniRx;

namespace BloodClockTower
{
    public class MappedReadOnlyReactiveProperty<TSource, TTarget>
        : IReadOnlyReactiveProperty<TTarget>
    {
        private readonly IReadOnlyReactiveProperty<TSource> _reactiveProperty;
        private readonly Func<TSource, TTarget> _mapper;

        public MappedReadOnlyReactiveProperty(
            IReadOnlyReactiveProperty<TSource> reactiveProperty,
            Func<TSource, TTarget> mapper
        )
        {
            _reactiveProperty = reactiveProperty;
            _mapper = mapper;
        }

        public TTarget Value => _mapper.Invoke(_reactiveProperty.Value);

        public bool HasValue => _reactiveProperty.HasValue;

        public IDisposable Subscribe(IObserver<TTarget> observer)
        {
            return _reactiveProperty.Select(_mapper).Subscribe(observer);
        }
    }
}
