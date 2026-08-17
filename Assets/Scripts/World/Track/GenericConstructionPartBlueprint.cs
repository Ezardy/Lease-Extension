using UnityEngine;
using Zenject;

namespace Aniki.World {
	internal class ConstructionPartBlueprint<T, F>
		: ConstructionPartBlueprint where T : MonoBehaviour where F : IFactory<Object, T> {
		private F	factory;

		[Inject]
		public virtual void	Init(F factory) {
			this.factory = factory;
		}

		protected override GameObject	CreateInstance() {
			return factory.Create(prefab).gameObject;
		}
	}
}
