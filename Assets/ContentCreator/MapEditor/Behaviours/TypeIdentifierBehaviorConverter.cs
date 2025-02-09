﻿using System.Collections.Generic;

using ProjectXyz.Api.GameObjects.Behaviors;
using ProjectXyz.Plugins.Features.CommonBehaviors;
using ProjectXyz.Plugins.Features.CommonBehaviors.Api;
using ProjectXyz.Shared.Framework;

using UnityEngine;

namespace Assets.ContentCreator.MapEditor.Behaviours
{
    public sealed class TypeIdentifierBehaviorConverter : IDiscoverableBehaviorConverter
    {
        public bool CanConvert(IBehavior behavior) => behavior is ITypeIdentifierBehavior;

        public bool CanConvert(Component component) => component is TypeIdentifierBehaviour;

        public IEnumerable<Component> Convert(
            GameObject target,
            IBehavior behavior)
        {
            var castedBehavior = (ITypeIdentifierBehavior)behavior;
            var component = target.AddComponent<TypeIdentifierBehaviour>();
            component.TypeId = castedBehavior.TypeId.ToString();
            yield return  component;
        }

        public IEnumerable<IBehavior> Convert(Component component)
        {
            var castedBehaviour = (TypeIdentifierBehaviour)component;
            var id = new StringIdentifier(castedBehaviour.TypeId);
            var behavior = new TypeIdentifierBehavior(id);
            yield return behavior;
        }
    }
}
