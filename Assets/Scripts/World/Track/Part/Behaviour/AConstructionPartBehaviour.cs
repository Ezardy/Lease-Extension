using UnityEngine;
using Zenject;

namespace LeaseExtension.World.Track.Part.Behaviour
{
    internal abstract class AConstructionPartBehaviour : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<Object, AConstructionPartBehaviour>
        {
        }
    }
}
