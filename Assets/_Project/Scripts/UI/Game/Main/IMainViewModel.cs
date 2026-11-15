using UnityEngine.UIElements;

namespace LeaseExtension.UI.Game.Main
{
    internal interface IMainViewModel
    {
        public StyleEnum<DisplayStyle> MainDisplayStyle { get; }
        public uint Record { get; }
    }
}