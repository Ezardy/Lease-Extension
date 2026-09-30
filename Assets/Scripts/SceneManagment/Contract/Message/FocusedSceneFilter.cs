using Aniki.Common;

namespace Aniki.SceneManagment {
	public class FocusedSceneFilter : EqualityFilter<FocusedScene> {
		public static readonly FocusedSceneFilter	Welcome = new(FocusedScene.WELCOME);
		public static readonly FocusedSceneFilter	Main = new(FocusedScene.MAIN);

		public FocusedSceneFilter(FocusedScene sample) : base(sample) { }
	}
}
