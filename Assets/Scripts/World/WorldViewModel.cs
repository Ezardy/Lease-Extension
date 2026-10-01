using System;
using LeaseExtension.World.Contract;
using R3;
using UnityEngine;

namespace LeaseExtension.World
{
    internal class WorldViewModel : IDisposable
    {
        private readonly IDisposable _disposable;

        public WorldViewModel(IWorldModel model)
        {
            _disposable = model.GravityChanged.Subscribe(g => Physics2D.gravity = new(0, g));
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
