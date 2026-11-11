using System.Collections.Generic;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace LeaseExtension.Shop.Contract
{
    public static class ProductModelExtensions
    {
        public static bool IsIdentifiedBy(this IProductModel product, string id)
        {
            bool matches = product.GUID == id;
            if (!matches && !string.IsNullOrEmpty(id))
            {
                foreach (IResourceLocator locator in Addressables.ResourceLocators)
                {
                    if (locator.Locate(product.GUID, typeof(IProductModel), out IList<IResourceLocation> locations)
                        && locator.Locate(id, typeof(IProductModel), out IList<IResourceLocation> savedLocations))
                    {
                        matches |= locations.Any(location => savedLocations.Any(saved => saved.InternalId == location.InternalId));
                    }
                }
            }

            return matches;
        }

        public static bool IsOwnedBy(this IProductModel product, ITargetProductListModel products)
        {
            return products.OwnedProductIds.Any(product.IsIdentifiedBy);
        }
    }
}
