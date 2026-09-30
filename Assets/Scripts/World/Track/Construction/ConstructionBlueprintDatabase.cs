using System.Collections;
using System.Collections.Generic;
using LeaseExtension.Common.Utilities;
using LeaseExtension.World.Contract;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;

namespace LeaseExtension.World.Track.Construction
{
    [CreateAssetMenu(fileName = "ConstructionDatabase", menuName = "Scriptable Objects/Construction Database")]
    [MovedFrom("Aniki.World")]
    internal class ConstructionBlueprintDatabase : ScriptableObject, IConstructionBlueprintDatabase, IReadOnlyCollection<IConstructionBlueprint>
    {
        [SerializeField]
        [FormerlySerializedAs("constructions")]
        private List<IRef<IConstructionBlueprint>> _constructions;

        public int Count => _constructions.Count;
        public IReadOnlyCollection<IConstructionBlueprint> Constructions => this;

        public IEnumerator<IConstructionBlueprint> GetEnumerator()
        {
            return new RefEnumerator<IConstructionBlueprint>(_constructions);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
