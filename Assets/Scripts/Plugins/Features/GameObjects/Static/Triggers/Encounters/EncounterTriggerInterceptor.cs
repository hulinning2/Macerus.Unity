﻿using System.Linq;

using Assets.Scripts.Plugins.Features.GameObjects.Common.Api;

using Macerus.Plugins.Features.Encounters.GamObjects.Static.Triggers;

using ProjectXyz.Api.Behaviors;
using ProjectXyz.Api.GameObjects;

using UnityEngine;

namespace Assets.Scripts.Plugins.Features.GameObjects.Static.Triggers
{
    public sealed class EncounterTriggerInterceptor : IDiscoverableGameObjectBehaviorInterceptor
    {
        private readonly IEncounterTriggerBehaviourStitcher _encounterTriggerBehaviourStitcher;

        public EncounterTriggerInterceptor(IEncounterTriggerBehaviourStitcher encounterTriggerBehaviourStitcher)
        {
            _encounterTriggerBehaviourStitcher = encounterTriggerBehaviourStitcher;
        }

        public void Intercept(
            IGameObject gameObject,
            GameObject unityGameObject)
        {
            var properties = gameObject
                .Get<IReadOnlyEncounterTriggerPropertiesBehavior>()
                .SingleOrDefault();
            if (properties == null)
            {
                return;
            }

            _encounterTriggerBehaviourStitcher.Stitch(
                unityGameObject,
                properties);
        }
    }
}
