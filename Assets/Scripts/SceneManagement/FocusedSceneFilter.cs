using System;
using LeaseExtension.SceneManagement.Contract.Message;
using MessagePipe;

namespace LeaseExtension.SceneManagement
{
    internal class FocusedSceneFilter : MessageHandlerFilter<FocusedScene>
    {
        private readonly FocusedScene _scene;

        public FocusedSceneFilter(FocusedScene scene)
        {
            _scene = scene;
        }
        
        public override void Handle(FocusedScene message, Action<FocusedScene> next)
        {
            if (_scene == message)
            {
                next(message);
            }
        }
    }
}
