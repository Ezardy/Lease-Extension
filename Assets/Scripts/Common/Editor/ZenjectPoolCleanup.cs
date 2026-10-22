using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace LeaseExtension.Common.Editor
{
    [InitializeOnLoad]
    internal static class ZenjectPoolCleanup
    {
        private static readonly HashSet<IMemoryPool> _pools = new HashSet<IMemoryPool>();
        private static readonly HashSet<DiContainer> _containers = new HashSet<DiContainer>();
        private static readonly FieldInfo _childBindingsField = typeof(DiContainer).GetField(
            "_childBindings", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly FieldInfo _currentBindingsField = typeof(DiContainer).GetField(
            "_currentBindings", BindingFlags.Instance | BindingFlags.NonPublic);
        private static bool _isTracking = EditorApplication.isPlayingOrWillChangePlaymode;

        static ZenjectPoolCleanup()
        {
            StaticMemoryPoolRegistry.PoolAdded += OnPoolAdded;
            StaticMemoryPoolRegistry.PoolRemoved += OnPoolRemoved;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void TrackProjectContainer()
        {
            if (_isTracking && ProjectContext.HasInstance)
            {
                _containers.Add(ProjectContext.Instance.Container);
            }
        }

        private static void OnPoolAdded(IMemoryPool pool)
        {
            if (_isTracking && IsContainerPool(pool))
            {
                _pools.Add(pool);
            }
        }

        private static void OnPoolRemoved(IMemoryPool pool)
        {
            _pools.Remove(pool);
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                DisposePools();
                ReleaseBindingStatements();
                _isTracking = true;
            }
            else if (state == PlayModeStateChange.EnteredPlayMode || state == PlayModeStateChange.ExitingPlayMode)
            {
                // ProjectContext clears its static events at the start of every Play session.
                TrackProjectContainer();
            }
            else if (state == PlayModeStateChange.EnteredEditMode)
            {
                _isTracking = false;
            }
        }

        private static void DisposePools()
        {
            // Preserve the previous session's counts until the next Play session starts.
            IMemoryPool[] pools = new IMemoryPool[_pools.Count];
            _pools.CopyTo(pools);
            _pools.Clear();

            foreach (IMemoryPool pool in pools)
            {
                try
                {
                    ((IDisposable)pool).Dispose();
                }
                catch (Exception exception)
                {
                    Debug.LogException(exception);
                }
            }
        }

        private static void ReleaseBindingStatements()
        {
            foreach (DiContainer container in _containers)
            {
                try
                {
                    // Zenject has no public API to release inherited or unfinished bindings.
                    List<BindStatement> childBindings = (List<BindStatement>)_childBindingsField.GetValue(container);
                    Queue<BindStatement> currentBindings =
                        (Queue<BindStatement>)_currentBindingsField.GetValue(container);

                    foreach (BindStatement binding in childBindings)
                    {
                        binding.Dispose();
                    }

                    childBindings.Clear();

                    while (currentBindings.Count > 0)
                    {
                        currentBindings.Dequeue().Dispose();
                    }
                }
                catch (Exception exception)
                {
                    Debug.LogException(exception);
                }
            }

            _containers.Clear();
        }

        private static bool IsContainerPool(IMemoryPool pool)
        {
            bool isContainerPool = false;
            Type type = pool.GetType();

            while (type != null && !isContainerPool)
            {
                if (type.IsGenericType)
                {
                    Type genericType = type.GetGenericTypeDefinition();
                    isContainerPool = genericType == typeof(MemoryPoolBase<>) ||
                        genericType == typeof(MemoryPoolBaseAsync<>);
                }

                type = type.BaseType;
            }

            return isContainerPool;
        }
    }
}
