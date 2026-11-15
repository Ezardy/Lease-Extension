using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using LeaseExtension.Cosmetics.Contract;
using LeaseExtension.Shop.Contract;
using LeaseExtension.Wallet.Contract;

namespace LeaseExtension.Shop
{
    [UsedImplicitly]
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

        public bool IsOwned(CosmeticsTarget target, IProductModel model)
        {
            return _products[target].OwnedProductIds.Contains(model.GUID);
        }

        public bool TryAcquire(CosmeticsTarget target, IProductModel product)
        {
            ITargetProductListModel products = _products[target];
            bool acquired = products.OwnedProductIds.Contains(product.GUID);
            if (!acquired && _wallet.Withdraw(product.Price))
            {
                products.Own(product.GUID);
                acquired = true;
            }

            return acquired;
        }
    }
}