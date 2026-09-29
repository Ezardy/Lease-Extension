using UnityEngine;

namespace Aniki.World
{
	[CreateAssetMenu(fileName = "BehaviourConstructionPartBlueprint", menuName = "Scriptable Objects/Blueprints/Behaviour Construction Part Blueprint")]
	internal class BehaviourConstructionPartBlueprint
		: ConstructionPartBlueprint<AConstructionPartBehaviour, AConstructionPartBehaviour.Factory> { }
}
