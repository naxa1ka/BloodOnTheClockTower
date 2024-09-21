using Nxlk.UIToolkit;
using UnityEngine.UIElements;

namespace BloodClockTower.Game
{
    public class PlayerIconView
    {
        public VisualElement Container { get; }
        public VisualElement Icon { get; }
        public VisualElement BorderInner { get; }
        public VisualElement KilledBorder { get; }
        public VisualElement BorderOuter { get; }
        public VisualElement ArrowsNominee { get; }
        public VisualElement ArrowsInitiator { get; }
        public MultiLabel NameLabel { get; }

        public PlayerIconView(VisualElement visualElement)
        {
            Container = visualElement.Q("container");
            Icon = visualElement.Q("icon");
            BorderInner = visualElement.Q("border-inner");
            KilledBorder = visualElement.Q("killed-border");
            BorderOuter = visualElement.Q("border-outer");
            ArrowsNominee = visualElement.Q("arrows-nominee");
            ArrowsInitiator = visualElement.Q("arrows-initiator");
            NameLabel = visualElement.Q<MultiLabel>("name-label");
        }
    }
}
