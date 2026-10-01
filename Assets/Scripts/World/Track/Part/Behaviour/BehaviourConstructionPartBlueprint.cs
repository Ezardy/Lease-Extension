using LeaseExtension.World.Track.Part.Blueprint;
using UnityEngine;

namespace LeaseExtension.World.Track.Part.Behaviour
{
    [CreateAssetMenu(fileName = "BehaviourConstructionPartBlueprint", menuName = "Scriptable Objects/Blueprints/Behaviour Construction Part Blueprint")]
    internal class BehaviourConstructionPartBlueprint : ConstructionPartBlueprint<AConstructionPartBehaviour, AConstructionPartBehaviour.Factory>
    {
    }
}
