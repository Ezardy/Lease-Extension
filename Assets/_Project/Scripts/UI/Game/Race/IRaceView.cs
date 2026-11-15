using LeaseExtension.UI.Contract;
using UnityEngine;

namespace LeaseExtension.UI.Game.Race
{
    internal interface IRaceView : IView
    {
        public void AddEffect(string id, float duration, Texture texture);
    }
}