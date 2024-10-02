using VContainer.Unity;

#if VCONTAINER_INTEGRATION
namespace Nxlk.UIToolkit
{
    public interface IPresenter : IStartable
    {
        void IStartable.Start() => Initialize();

        void Initialize();
    }
}
#else
namespace Nxlk.UIToolkit
{
    public interface IPresenter 
    {
        void Initialize();
    }
}
#endif