using System.Collections.Generic;
using LeaseExtension.Common.Utilities;
using ObservableCollections;
using R3;
using UnityEngine.Scripting.APIUpdating;

namespace LeaseExtension.Cosmetics.Contract
{
    [MovedFrom("Aniki.Cosmetics")]
    internal abstract class ACosmeticsDecorator<T>
        where T : class
    {
        public class Database : ICosmeticsItemModelDatabase, ITyped<T>
        {
            private readonly ICosmeticsItemModelDatabase _database;

            public Database(ICosmeticsItemModelDatabase database)
            {
                _database = database;
            }

            public IEnumerable<ICosmeticsItemModel> GetCosmeticsItemModels()
            {
                return _database.GetCosmeticsItemModels();
            }

            public ICosmeticsItemModel GetCosmeticsItemModel(string id)
            {
                return _database.GetCosmeticsItemModel(id);
            }
        }

        public class ElementModel : ICosmeticsElementModel, ITyped<T>
        {
            private readonly ICosmeticsElementModel _model;

            public string Id
            {
                get => _model.Id;
                set => _model.Id = value;
            }
            public IReadOnlyCollection<string> IdCollection => _model.IdCollection;
            public Observable<string> IdChanged => _model.IdChanged;
            public Observable<CollectionAddEvent<string>> IdCollectionChanged => _model.IdCollectionChanged;

            public ElementModel(ICosmeticsElementModel model)
            {
                _model = model;
            }

            public void AddId(in string hair)
            {
                _model.AddId(in hair);
            }
        }
    }
}
