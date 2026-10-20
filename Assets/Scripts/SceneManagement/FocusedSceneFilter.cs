using LeaseExtension.Common.Utilities;
using LeaseExtension.SceneManagement.Contract.Message;

namespace LeaseExtension.SceneManagement
{
    internal class FocusedSceneFilter : EqualityFilter<FocusedScene>
    {
        public FocusedSceneFilter(FocusedScene sample) : base(sample)
        {}
    }
}
