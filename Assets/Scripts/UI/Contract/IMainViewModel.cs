using UnityEngine.UIElements;

namespace LeaseExtension.UI.Contract
{
    internal interface IMainViewModel
    {
        public StyleEnum<DisplayStyle> MainDisplayStyle { get; }
        public uint Record { get; }
    }
}
