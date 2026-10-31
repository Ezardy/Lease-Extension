using JetBrains.Annotations;

namespace LeaseExtension.Audio.Background.States
{
    [UsedImplicitly]
    internal class ShopAudioState : BackgroundAudioState<ShopAudioState>
    {
        public ShopAudioState(IBackgroundAudioContext context) : base(context, "Shop")
        {}
    }
}