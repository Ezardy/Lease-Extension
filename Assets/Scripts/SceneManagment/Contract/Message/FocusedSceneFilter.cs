using LeaseExtension.Common.Utilities;
using UnityEngine.Scripting.APIUpdating;

namespace LeaseExtension.SceneManagment.Contract.Message
{
    [MovedFrom("Aniki.SceneManagment")]
    public class FocusedSceneFilter : EqualityFilter<FocusedScene>
    {
        public static readonly FocusedSceneFilter Welcome = new(FocusedScene.Welcome);
        public static readonly FocusedSceneFilter Main = new(FocusedScene.Main);

        public FocusedSceneFilter(FocusedScene sample) : base(sample)
        {
        }
    }
}
