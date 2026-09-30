using UnityEngine.UIElements;

namespace LeaseExtension.UI.Contract
{
    internal interface IRaceViewModel
    {
        public StyleEnum<DisplayStyle> GameDisplayStyle { get; }
        public uint BarsPassed { get; }
        public uint Earned { get; }
    }
}
