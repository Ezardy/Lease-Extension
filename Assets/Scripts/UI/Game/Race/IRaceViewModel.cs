using UnityEngine.UIElements;

namespace LeaseExtension.UI.Game.Race
{
    internal interface IRaceViewModel
    {
        public StyleEnum<DisplayStyle> GameDisplayStyle { get; }
        public uint BarsPassed { get; }
        public uint Earned { get; }
    }
}