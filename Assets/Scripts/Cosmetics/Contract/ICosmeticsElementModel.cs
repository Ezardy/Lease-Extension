using System.Collections.Generic;
using ObservableCollections;
using R3;

namespace LeaseExtension.Cosmetics.Contract
{
    public interface ICosmeticsElementModel
    {
        public string Id { get; set; }

        public IReadOnlyCollection<string> IdCollection { get; }
        public Observable<string> IdChanged { get; }
        public Observable<CollectionAddEvent<string>> IdCollectionChanged { get; }

        public void AddId(in string hair);
    }
}
