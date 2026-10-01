using LeaseExtension.Common.Utilities;

namespace LeaseExtension.SceneManagment.Contract.Message
{
    public class FocusedSceneFilter : EqualityFilter<FocusedScene>
    {
        public static readonly FocusedSceneFilter Welcome = new(FocusedScene.Welcome);
        public static readonly FocusedSceneFilter Main = new(FocusedScene.Main);

        public FocusedSceneFilter(FocusedScene sample) : base(sample)
        {
        }
    }
}
