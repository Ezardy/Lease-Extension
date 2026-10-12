using System.Collections.Generic;
using UnityEngine;

namespace LeaseExtension.World.Track.Construction
{
    [CreateAssetMenu(fileName = "ConstructionDatabase", menuName = "Scriptable Objects/Construction Database")]
    internal class ConstructionBlueprintDatabaseSettings : ScriptableObject
    {
        [SerializeField] private List<ConstructionBlueprintSettings> _constructionSettings;

        public IEnumerable<ConstructionBlueprintSettings> ConstructionSettings => _constructionSettings;
    }
}
