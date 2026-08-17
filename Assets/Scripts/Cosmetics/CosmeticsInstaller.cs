using Aniki.Common;
using Aniki.Save;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Aniki.Cosmetics {
	[CreateAssetMenu(fileName = "CosmeticsInstaller", menuName = "Installers/Cosmetics Installer")]
	internal class CosmeticsInstaller : ScriptableObjectInstaller<CosmeticsInstaller> {
		[SerializeField] private CosmeticsElementModelDependencies	babHairstyleModelDependencies;
		[SerializeField] private CosmeticsElementModelDependencies	pupHairstyleModelDependencies;

		public override void	InstallBindings() {
			Install<BabHairstyle, BabHairstyle.ElementModel, BabHairstyle.Database>(
				babHairstyleModelDependencies.cosmeticsSave,
				babHairstyleModelDependencies.cosmeticsCollectionSave,
				babHairstyleModelDependencies.database.I
			);
			Install<PupHairstyle, PupHairstyle.ElementModel, PupHairstyle.Database>(
				pupHairstyleModelDependencies.cosmeticsSave,
				pupHairstyleModelDependencies.cosmeticsCollectionSave,
				pupHairstyleModelDependencies.database.I
			);
		}

		private void	Install<T, M, D>(CosmeticsSave save,
			CosmeticsCollectionSave collectionSave,
			ICosmeticsItemModelDatabase database)
			where M : ICosmeticsElementModel
			where D : ICosmeticsItemModelDatabase
			where T : class {
			Container.Bind<ICosmeticsElementModel>().FromSubContainerResolve().ByMethod(
				c => InstallHairstyle<BabHairstyle.ElementModel>(
					c, save, collectionSave
				)
			).AsCached().WithTypeOrAny<T>();

			Container.Bind<ICosmeticsItemModelDatabase>().FromSubContainerResolve().ByMethod(
				c => InstallDatabase<BabHairstyle.Database>(c, database)
			).AsCached().WithTypeOrAny<T>();
		}

		private void	InstallHairstyle<M>(DiContainer container,
			CosmeticsSave cosmeticsSave,
			CosmeticsCollectionSave cosmeticsCollectionSave) where M : ICosmeticsElementModel {
			cosmeticsSave.Load();
			cosmeticsCollectionSave.Load();

			container.BindInterfacesTo<CosmeticsElementModel>().AsSingle().WithArguments(cosmeticsSave.Data.id, cosmeticsCollectionSave.Data.idCollection);
			container.BindInterfacesTo<CosmeticsSave>().FromInstance(cosmeticsSave);
			container.BindInterfacesTo<CosmeticsCollectionSave>().FromInstance(cosmeticsCollectionSave);
			container.QueueForInject(cosmeticsSave);
			container.QueueForInject(cosmeticsCollectionSave);

			container.Decorate<ICosmeticsElementModel>().With<M>();
		}

		private void	InstallDatabase<D>(DiContainer container, ICosmeticsItemModelDatabase database) where D : ICosmeticsItemModelDatabase {
			container.BindInstance(database);
			container.Decorate<ICosmeticsItemModelDatabase>().With<D>();
		}

		[Serializable]
		private class CosmeticsElementModelDependencies {
			public CosmeticsSave						cosmeticsSave;
			public CosmeticsCollectionSave				cosmeticsCollectionSave;
			public IRef<ICosmeticsItemModelDatabase>	database;
		}
	}
}