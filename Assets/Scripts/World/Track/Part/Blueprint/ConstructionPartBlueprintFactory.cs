using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using LeaseExtension.World.Contract;
using Zenject;

namespace LeaseExtension.World.Track.Part.Blueprint
{
    internal class ConstructionPartBlueprintFactory : IFactory<IConstructionPartBlueprintSettings, IConstructionPartBlueprint>
    {
        private readonly Dictionary<Type, IConstructionPartBlueprintFactory> _factories = new();
        private readonly Dictionary<IConstructionPartBlueprintSettings, IConstructionPartBlueprint> _blueprints = new();
        private readonly HashSet<IConstructionPartBlueprintSettings> _creating = new();
        private readonly HashSet<IConstructionPartBlueprintSettings> _installed = new();

        public ConstructionPartBlueprintFactory(IReadOnlyCollection<IConstructionPartBlueprintFactory> factories)
        {
            foreach (IConstructionPartBlueprintFactory factory in factories)
            {
                _factories.Add(factory.SettingsType, factory);
            }
        }

        public void InstallPools(IConstructionPartBlueprintSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            if (_installed.Add(settings))
            {
                GetFactory(settings).InstallPools(settings);
                foreach (IConstructionPartBlueprintSettings subPart in settings.SubPartSettings)
                {
                    InstallPools(subPart);
                }
            }
        }

        public IConstructionPartBlueprint Create(IConstructionPartBlueprintSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            if (!_blueprints.TryGetValue(settings, out IConstructionPartBlueprint blueprint))
            {
                if (!_creating.Add(settings))
                {
                    throw new InvalidOperationException("Construction part settings contain a cyclic reference.");
                }
                try
                {
                    List<IConstructionPartBlueprint> subParts = new();
                    foreach (IConstructionPartBlueprintSettings subPartSettings in settings.SubPartSettings)
                    {
                        subParts.Add(Create(subPartSettings));
                    }
                    blueprint = GetFactory(settings).Create(settings, subParts.AsReadOnly());
                    _blueprints.Add(settings, blueprint);
                }
                finally
                {
                    _creating.Remove(settings);
                }
            }
            return blueprint;
        }

        private IConstructionPartBlueprintFactory GetFactory(IConstructionPartBlueprintSettings settings)
        {
            Type settingsType = settings.GetType();
            IConstructionPartBlueprintFactory factory = null;
            while (settingsType != null && !_factories.TryGetValue(settingsType, out factory))
            {
                settingsType = settingsType.BaseType;
            }
            if (factory == null)
            {
                throw new InvalidOperationException($"No construction part blueprint factory is registered for {settings.GetType().Name}.");
            }
            return factory;
        }

        [UsedImplicitly]
        public class Factory : PlaceholderFactory<IConstructionPartBlueprintSettings, IConstructionPartBlueprint>
        {
        }
    }
}
