using System;
using System.Collections.Generic;
using LeaseExtension.Shop.Contract;
using ObservableCollections;
using R3;
using UnityEngine.AddressableAssets;

namespace LeaseExtension.Shop.Model
{
    internal class TargetProductListModel : ITargetProductListModel
    {
        private readonly ObservableHashSet<string> _ownedProductIds;

        public IReadOnlyCollection<AssetReference> Products { get; }
        public IReadOnlyCollection<string> OwnedProductIds => _ownedProductIds;
        public Observable<CollectionAddEvent<string>> ProductOwned => _ownedProductIds.ObserveAdd();

        public TargetProductListModel(
            IReadOnlyCollection<AssetReference> products,
            IEnumerable<string> ownedProductIds)
        {
            Products = products ?? Array.Empty<AssetReference>();
            _ownedProductIds = new(ownedProductIds ?? Array.Empty<string>());
        }

        public void Own(in string productId)
        {
            _ownedProductIds.Add(productId);
        }
    }
}