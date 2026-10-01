using System.Collections.Generic;
using LeaseExtension.Cosmetics.Contract;
using ObservableCollections;
using R3;

namespace LeaseExtension.Cosmetics
{
    internal class CosmeticsElementModel : ICosmeticsElementModel
    {
        private readonly ReactiveProperty<string> _id;
        private readonly ObservableHashSet<string> _idCollection;

        public string Id
        {
            get => _id.CurrentValue;
            set
            {
                if (_idCollection.Contains(value))
                    _id.Value = value;
            }
        }

        public IReadOnlyCollection<string> IdCollection => _idCollection;
        public Observable<string> IdChanged => _id;
        public Observable<CollectionAddEvent<string>> IdCollectionChanged => _idCollection.ObserveAdd();

        public CosmeticsElementModel(string id, IReadOnlyCollection<string> idCollection)
        {
            _id = new(id);
            _idCollection = new(idCollection);
        }

        public void AddId(in string id)
        {
            _idCollection.Add(id);
        }
    }
}
