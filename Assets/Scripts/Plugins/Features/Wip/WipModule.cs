﻿using System.Collections.Generic;
using System.Linq;

using Autofac;

using Macerus.Api.Behaviors;
using Macerus.Plugins.Features.GameObjects.Skills.Api;

using ProjectXyz.Api.Behaviors;
using ProjectXyz.Api.Behaviors.Filtering;
using ProjectXyz.Api.Behaviors.Filtering.Attributes;
using ProjectXyz.Api.Enchantments;
using ProjectXyz.Api.Enchantments.Calculations;
using ProjectXyz.Api.Enchantments.Generation;
using ProjectXyz.Api.Enchantments.Stats;
using ProjectXyz.Api.Framework.Entities;
using ProjectXyz.Api.GameObjects;
using ProjectXyz.Api.Logging;
using ProjectXyz.Api.Stats;
using ProjectXyz.Plugins.Features.Behaviors.Filtering.Default.Attributes;
using ProjectXyz.Plugins.Features.CommonBehaviors;
using ProjectXyz.Plugins.Features.CommonBehaviors.Api;
using ProjectXyz.Plugins.Features.GameObjects.Actors.Api;
using ProjectXyz.Plugins.Features.GameObjects.Enchantments.Default.Calculations;
using ProjectXyz.Plugins.Features.GameObjects.Skills;
using ProjectXyz.Plugins.Features.GameObjects.StatCalculation.Api;
using ProjectXyz.Shared.Framework;

namespace Assets.Scripts.Plugins.Features.Wip
{
    public sealed class WipModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<PlayerTestingBehaviorsInterceptor>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }

    public sealed class PlayerTestingBehaviorsInterceptor : IDiscoverableActorBehaviorsInterceptor
    {
        private readonly IReadOnlyStatDefinitionToTermMappingRepository _statDefinitionToTermMappingRepository;
        private readonly IFilterContextFactory _filterContextFactory;
        private readonly ISkillAmenity _skillAmenity;

        public PlayerTestingBehaviorsInterceptor(
            IReadOnlyStatDefinitionToTermMappingRepository statDefinitionToTermMappingRepository,
            IFilterContextFactory filterContextFactory,
            ISkillAmenity skillAmenity)
        {
            _statDefinitionToTermMappingRepository = statDefinitionToTermMappingRepository;
            _filterContextFactory = filterContextFactory;
            _skillAmenity = skillAmenity;
        }

        public void Intercept(IReadOnlyCollection<IBehavior> behaviors)
        {
            if (!behaviors.Has<IPlayerControlledBehavior>())
            {
                return;
            }

            var mutableStats = behaviors.GetOnly<IHasMutableStatsBehavior>();
            mutableStats.MutateStats(stats =>
            {
                stats[_statDefinitionToTermMappingRepository.GetStatDefinitionToTermMappingByTerm("LIFE_MAXIMUM").StatDefinitionId] = 100;
                stats[_statDefinitionToTermMappingRepository.GetStatDefinitionToTermMappingByTerm("LIFE_CURRENT").StatDefinitionId] = 10;
                stats[_statDefinitionToTermMappingRepository.GetStatDefinitionToTermMappingByTerm("MANA_MAXIMUM").StatDefinitionId] = 100;
                stats[_statDefinitionToTermMappingRepository.GetStatDefinitionToTermMappingByTerm("MANA_CURRENT").StatDefinitionId] = 100;
            });

            var skillsBehavior = behaviors.GetOnly<IHasSkillsBehavior>();
            skillsBehavior.Add(new[]
            {
                _skillAmenity.GetSkillById(new StringIdentifier("heal-self")),
            });
        }
    }
}
