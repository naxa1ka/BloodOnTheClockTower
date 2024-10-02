using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nxlk.UIToolkit
{
    [Serializable]
    public abstract class SerializableSafetyUiDocument : ISafetyUiDocument
    {
        [SerializeField] private UIDocument _uiDocument = null!;

        private readonly Lazy<SafetyUiDocument> _lazySafetyUiDocument;

        protected SerializableSafetyUiDocument()
        {
            _lazySafetyUiDocument = new Lazy<SafetyUiDocument>(() =>
            {
                if (_uiDocument == null)
                    throw new NullReferenceException("The UI Document is null");
                return _uiDocument.ToSafetyUiDocument();
            });
        }

        public T Q<T>(string? name = null) where T : VisualElement => _lazySafetyUiDocument.Value.Q<T>(name);
    }
}