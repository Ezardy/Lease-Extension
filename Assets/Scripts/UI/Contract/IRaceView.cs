using UnityEngine;

namespace LeaseExtension.UI.Contract
{
    internal interface IRaceView : IView
    {
        public void AddEffect(string id, float duration, Texture texture);
    }
}
