using System.Collections.Generic;
using System.Linq;
using LeaseExtension.Cosmetics.Contract;
using LeaseExtension.Shop.Contract;
using LeaseExtension.Wallet.Contract;

namespace LeaseExtension.Shop
{
    internal class Shop : IShop
    {
        private readonly IReadOnlyDictionary<CosmeticsTarget, ITargetProductListModel> _products;
        private readonly IWalletModel _wallet;

        public Shop(
            IReadOnlyDictionary<CosmeticsTarget, ITargetProductListModel> products,
            IWalletModel wallet)
        {
            _products = products;
            _wallet = wallet;
        }

        public bool TryAcquire(CosmeticsTarget target, IProductModel product, string productId)
        {
            ITargetProductListModel products = _products[target];
            bool acquired = false;
            if (products.Products.Any(reference => reference.AssetGUID == product.GUID))
            {
                bool owned = products.OwnedProductIds.Contains(productId);
                if (!owned && _wallet.Withdraw(product.Price))
                {
                    products.Own(productId);
                    owned = true;
                }

                acquired = owned;
            }

            return acquired;
        }
    }
}
