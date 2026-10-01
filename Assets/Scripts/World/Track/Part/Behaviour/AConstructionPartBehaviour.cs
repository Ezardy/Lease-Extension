using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace LeaseExtension.World.Track.Part.Behaviour
{
    internal abstract class AConstructionPartBehaviour : MonoBehaviour
    {
        [UsedImplicitly]
        public class Factory : PlaceholderFactory<Object, AConstructionPartBehaviour>
        {
        }
    }
}
