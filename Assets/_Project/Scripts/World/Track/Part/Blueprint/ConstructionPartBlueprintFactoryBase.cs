using System;
using System.Collections.Generic;
using LeaseExtension.World.Contract;
using UnityEngine;
using Zenject;

namespace LeaseExtension.World.Track.Part.Blueprint
{
    internal abstract class ConstructionPartBlueprintFactoryBase<TSettings, TBlueprint> :
        IConstructionPartBlueprintFactory, IFactory<TSettings, IReadOnlyList<IConstructionPartBlueprint>, TBlueprint>
        where TSettings : ScriptableObject, IConstructionPartBlueprintSettings
        where TBlueprint : IConstructionPartBlueprint
    {
        protected readonly DiContainer Container;

        public Type SettingsType => typeof(TSettings);

        protected ConstructionPartBlueprintFactoryBase(DiContainer container)
        {
            Container = container;
        }

        public abstract TBlueprint Create(TSettings settings, IReadOnlyList<IConstructionPartBlueprint> subParts);

        public void InstallPools(IConstructionPartBlueprintSettings settings)
        {
            InstallPools((TSettings)settings);
        }

        public IConstructionPartBlueprint Create(IConstructionPartBlueprintSettings settings,
            IReadOnlyList<IConstructionPartBlueprint> subParts)
        {
            return Create((TSettings)settings, subParts);
        }

        protected virtual void InstallPools(TSettings settings)
        {}
    }
}