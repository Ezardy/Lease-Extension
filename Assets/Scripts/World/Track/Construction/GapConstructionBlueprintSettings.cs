using System.Collections.Generic;
using LeaseExtension.World.Contract;
using UnityEngine;

namespace LeaseExtension.World.Track.Construction
{
    [CreateAssetMenu(fileName = "GapConstructionBlueprint",
        menuName = "Scriptable Objects/Blueprints/Gap Construction Blueprint")]
    internal class GapConstructionBlueprintSettings : ConstructionBlueprintSettings
    {
        public override void MakeBlanks(IEnumerable<IConstructionPartBlueprint> parts,
            ICollection<IConstructionBlank> blanksOutput)
        {
            float size = 0;
            foreach (IConstructionPartBlueprint part in parts)
            {
                if (part is IGapConstructionPartBlueprint gap)
                {
                    size = Mathf.Max(size, gap.GapSize);
                }
            }

            float gapPos = Random.Range(size / 2, 1 - size / 2);
            foreach (IConstructionPartBlueprint part in parts)
            {
                if (part is IGapConstructionPartBlueprint gap)
                {
                    blanksOutput.Add(gap.MakeBlank(0, 1, gapPos));
                }
                else
                {
                    blanksOutput.Add(part.MakeBlank(0, 1));
                }
            }
        }
    }
}