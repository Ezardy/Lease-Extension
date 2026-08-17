using Aniki.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aniki.World {
	[CreateAssetMenu(fileName = "ConstructionDictionary", menuName = "Scriptable Objects/Construction Dictionary")]
	internal class ConstructionBlueprintDatabase : ScriptableObject, IConstructionBlueprintDatabase, IReadOnlyCollection<IConstructionBlueprint> {
		[SerializeField] private List<IRef<IConstructionBlueprint>>	constructions;

		public int Count => constructions.Count;

		public IReadOnlyCollection<IConstructionBlueprint> Constructions => this;

		public IEnumerator<IConstructionBlueprint>	GetEnumerator() {
			return new RefEnumerator<IConstructionBlueprint>(constructions);
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}
}
