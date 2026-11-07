using System.Collections.Generic;
using ObservableCollections;
using R3;
using UnityEngine.AddressableAssets;

namespace LeaseExtension.Shop.Contract
{
    public interface ITargetProductsModel
    {
        public IReadOnlyCollection<AssetReference> Products { get; }
        public IReadOnlyCollection<string> OwnedProductIds { get; }
        public Observable<CollectionAddEvent<string>> ProductOwned { get; }
        public void Own(in string hair);
    }
}