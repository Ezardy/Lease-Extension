using System;
using LeaseExtension.World.Contract;
using R3;
using UnityEngine.Scripting.APIUpdating;

namespace LeaseExtension.World.Wall
{
    [MovedFrom("Aniki.World")]
    internal class WallViewModel : IDisposable
    {
        private readonly IDisposable _disposable;

        public WallViewModel(IWallView view, IWorldModel model)
        {
            _disposable = model.SpeedChanged.Subscribe(
s => view.Speed = s * model.Perspective * 2 / (1 + model.Perspective));
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
