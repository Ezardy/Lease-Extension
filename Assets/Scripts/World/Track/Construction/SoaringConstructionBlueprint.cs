using System.Collections.Generic;
using LeaseExtension.World.Contract;
using UnityEngine;

namespace LeaseExtension.World.Track.Construction
{
    [CreateAssetMenu(fileName = "CollectableConstructionBlueprint", menuName = "Scriptable Objects/Blueprints/Soaring Construction Blueprint")]
    internal class SoaringConstructionBlueprint : ConstructionBlueprint
    {
        [SerializeField, Range(0, 1)]
        private float _startHeight = 0;
        [SerializeField, Range(0, 1)]
        private float _endHeight = 1;

        public override IReadOnlyCollection<IConstructionBlank> MakeBlanks()
        {
            Blanks.Clear();
            foreach (IConstructionPartBlueprint part in this)
                Blanks.Add(part.MakeBlank(Random.Range(_startHeight, _endHeight), Height));
            return Blanks;
        }
    }
}
