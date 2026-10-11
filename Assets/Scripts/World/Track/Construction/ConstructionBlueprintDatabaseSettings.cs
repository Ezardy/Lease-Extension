using System.Collections.Generic;
using LeaseExtension.Common.Utilities;
using UnityEngine;

namespace LeaseExtension.World.Track.Construction
{
    [CreateAssetMenu(fileName = "ConstructionDatabase", menuName = "Scriptable Objects/Construction Database")]
    internal class ConstructionBlueprintDatabaseSettings : ScriptableObject
    {
        [SerializeField] private List<Ref<ConstructionBlueprintSettings>> _constructions;

        public IEnumerable<ConstructionBlueprintSettings> Constructions
        {
            get
            {
                foreach (Ref<ConstructionBlueprintSettings> construction in _constructions)
                    yield return construction.I;
            }
        }
    }
}
