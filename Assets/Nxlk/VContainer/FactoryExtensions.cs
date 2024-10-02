using System;
using VContainer;

namespace Nxlk.VContainer
{
    public static class FactoryExtensions
    {
        public static RegistrationBuilder RegisterIFactory<TParam1, TValue>(this IContainerBuilder builder, Func<TParam1, TValue> factory)
        {
          return  builder.Register<IFactory<TParam1, TValue>>(
                _ => new Factory<TParam1, TValue>(factory),
                Lifetime.Singleton
            );
        }
    }
}