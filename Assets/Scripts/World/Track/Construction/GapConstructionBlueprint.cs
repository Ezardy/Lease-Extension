using System.Collections.Generic;
using LeaseExtension.World.Contract;
using UnityEngine;

namespace LeaseExtension.World.Track.Construction
{
    [CreateAssetMenu(fileName = "GapConstructionBlueprint", menuName = "Scriptable Objects/Blueprints/Gap Construction Blueprint")]
    internal class GapConstructionBlueprint : ConstructionBlueprint
    {
        public override IReadOnlyCollection<IConstructionBlank> MakeBlanks()
        {
            float size = 0;
            float gapPos;
            Blanks.Clear();
            foreach (IConstructionPartBlueprint part in this)
                if (part is IGapConstructionPartBlueprint gap)
                    size = Mathf.Max(size, gap.GapSize);
            gapPos = Random.Range(size / 2, 1 - size / 2);
            foreach (IConstructionPartBlueprint part in this)
                if (part is IGapConstructionPartBlueprint gap)
                    Blanks.Add(gap.MakeBlank(0, gapPos));
                else
                    Blanks.Add(part.MakeBlank(0, 1));
            return Blanks;
        }
    }
}
