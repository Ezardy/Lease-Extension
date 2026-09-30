using LeaseExtension.World.Track.Part.Blueprint;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace LeaseExtension.World.Track.Part.Behaviour
{
    [CreateAssetMenu(fileName = "BehaviourConstructionPartBlueprint", menuName = "Scriptable Objects/Blueprints/Behaviour Construction Part Blueprint")]
    [MovedFrom("Aniki.World")]
    internal class BehaviourConstructionPartBlueprint : ConstructionPartBlueprint<AConstructionPartBehaviour, AConstructionPartBehaviour.Factory>
    {
    }
}
