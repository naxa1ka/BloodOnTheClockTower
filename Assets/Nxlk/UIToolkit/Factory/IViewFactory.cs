namespace Nxlk.UIToolkit
{
    public interface IViewFactory<out T>
    {
        T Create();
    }
}