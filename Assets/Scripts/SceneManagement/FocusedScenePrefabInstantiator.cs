using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using LeaseExtension.Common.Utilities;
using LeaseExtension.SceneManagement.Contract.Message;
using R3;
using Object = UnityEngine.Object;

namespace LeaseExtension.SceneManagement
{
    [UsedImplicitly]
    internal class FocusedScenePrefabInstantiator : IDisposable
    {
        private readonly IDisposable _disposable;

        public FocusedScenePrefabInstantiator(FocusedScene scene, Observable<FocusedScene> sceneObservable,
            PrefabFactory.Factory sceneFactory, List<Object> prefabs)
        {
            _disposable = sceneObservable.Subscribe(s =>
            {
                if (s == scene)
                {
                    _disposable.Dispose();
                    foreach (Object prefab in prefabs)
                    {
                        sceneFactory.Create(prefab);
                    }
                }
            });
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}