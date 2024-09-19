using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Nxlk.UIToolkit
{
    public class MultiLabel : VisualElement
    {
        #region UXML

        public new class UxmlFactory : UxmlFactory<MultiLabel, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private readonly UxmlStringAttributeDescription _text =
                new() { name = "text", defaultValue = "text" };

            private readonly UxmlBoolAttributeDescription _enableRichText =
                new() { name = "enable-rich-text", defaultValue = true };

            private readonly UxmlBoolAttributeDescription _parseEscapeSequences =
                new() { name = "parse-escape-sequences", defaultValue = false };

            private readonly UxmlBoolAttributeDescription _displayTooltipWhenElided =
                new() { name = "display-tooltip-when-elision", };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var multiLabel = (MultiLabel)ve;
                multiLabel.Text = _text.GetValueFromBag(bag, cc);
                multiLabel.EnableRichText = _enableRichText.GetValueFromBag(bag, cc);
                multiLabel.ParseEscapeSequences = _parseEscapeSequences.GetValueFromBag(bag, cc);
                multiLabel.DisplayTooltipWhenElided = _displayTooltipWhenElided.GetValueFromBag(
                    bag,
                    cc
                );
            }
        }

        #endregion

        private IEnumerable<Label> Labels => this.Query<Label>().ToList();

        private bool _displayTooltipWhenElided;
        public bool DisplayTooltipWhenElided
        {
            get => _displayTooltipWhenElided;
            set
            {
                _displayTooltipWhenElided = value;
                foreach (var label in Labels)
                    label.displayTooltipWhenElided = _displayTooltipWhenElided;
            }
        }

        private bool _parseEscapeSequences;
        public bool ParseEscapeSequences
        {
            get => _parseEscapeSequences;
            set
            {
                _parseEscapeSequences = value;
                foreach (var label in Labels)
                    label.parseEscapeSequences = _parseEscapeSequences;
            }
        }

        private string _text = string.Empty;
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                foreach (var label in Labels)
                    label.text = _text;
            }
        }

        private bool _enableRichText;
        public bool EnableRichText
        {
            get => _enableRichText;
            set
            {
                _enableRichText = value;
                foreach (var label in Labels)
                    label.enableRichText = _enableRichText;
            }
        }

        public void SetValueWithoutNotify(string text)
        {
            foreach (var label in Labels)
                ((INotifyValueChanged<string>)label).SetValueWithoutNotify(text);
        }
    }
}
