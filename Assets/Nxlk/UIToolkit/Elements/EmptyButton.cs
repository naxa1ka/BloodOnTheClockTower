using UnityEngine.UIElements;

namespace Nxlk.UIToolkit
{
    public class EmptyButton : Button 
    {
        #region UXML

        public new class UxmlFactory : UxmlFactory<EmptyButton, UxmlTraits> { }

        public new class UxmlTraits : Button.UxmlTraits
        {
        }

        public EmptyButton()
        {
            RemoveFromClassList(ussClassName);
            RemoveFromClassList(TextElement.ussClassName);
        }

        #endregion
    }
}