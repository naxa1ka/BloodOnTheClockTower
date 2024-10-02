#if VCONTAINER_INTEGRATION
using VContainer.Unity;
namespace Nxlk.Initialization
{
    public interface IInitializable : VContainer.Unity.IInitializable
    {
    }
}
#else
namespace Nxlk.Initialization
{
    public interface IInitializable
    {
        void Initialize();
    }
}
#endif