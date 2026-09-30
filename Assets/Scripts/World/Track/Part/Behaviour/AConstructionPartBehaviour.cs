using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using Zenject;

namespace LeaseExtension.World.Track.Part.Behaviour
{
    [MovedFrom("Aniki.World")]
    internal abstract class AConstructionPartBehaviour : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<Object, AConstructionPartBehaviour>
        {
        }
    }
}
