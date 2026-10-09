using UnityEngine.UIElements;

namespace LeaseExtension.UI.Contract
{
    internal interface IOverViewModel
    {
        public StyleEnum<DisplayStyle> OverDisplayStyle { get; }
        public StyleEnum<DisplayStyle> NewRecordDisplayStyle { get; }
        public uint BarsPassed { get; }
        public uint Earned { get; }
    }
}
