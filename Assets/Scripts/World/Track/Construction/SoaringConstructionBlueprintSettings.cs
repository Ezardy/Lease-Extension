using System.Collections.Generic;
using LeaseExtension.World.Contract;
using UnityEngine;

namespace LeaseExtension.World.Track.Construction
{
    [CreateAssetMenu(fileName = "CollectableConstructionBlueprint", menuName = "Scriptable Objects/Blueprints/Soaring Construction Blueprint")]
    internal class SoaringConstructionBlueprintSettings : ConstructionBlueprintSettings
    {
        [SerializeField, Range(0, 1)]
        private float _startHeight = 0;
        [SerializeField, Range(0, 1)]
        private float _endHeight = 1;

        public override void MakeBlanks(IEnumerable<IConstructionPartBlueprint> parts, ICollection<IConstructionBlank> blanksOutput)
        {
            foreach (IConstructionPartBlueprint part in parts)
                blanksOutput.Add(part.MakeBlank(Random.Range(_startHeight, _endHeight), Height));
        }
    }
}
