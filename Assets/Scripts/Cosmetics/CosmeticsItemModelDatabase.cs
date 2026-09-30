using System.Collections.Generic;
using System.Linq;
using LeaseExtension.Cosmetics.Contract;
using UnityEngine;
using UnityEngine.Serialization;

namespace LeaseExtension.Cosmetics
{
    [CreateAssetMenu(fileName = "CosmeticsDatabase", menuName = "Scriptable Objects/Cosmetics Database")]
    internal class CosmeticsItemModelDatabase : ScriptableObject, ICosmeticsItemModelDatabase
    {
        [SerializeField]
        [FormerlySerializedAs("variants")]
        private List<CosmeticsItemModel> _variants;
        private Dictionary<string, CosmeticsItemModel> _variantsDict;

        private void OnEnable()
        {
            if (_variants != null)
                _variantsDict = _variants.ToDictionary(p => p.Id);
        }

        public ICosmeticsItemModel GetCosmeticsItemModel(string id)
        {
            return _variantsDict[id];
        }

        public IEnumerable<ICosmeticsItemModel> GetCosmeticsItemModels()
        {
            return _variants;
        }
    }
}
