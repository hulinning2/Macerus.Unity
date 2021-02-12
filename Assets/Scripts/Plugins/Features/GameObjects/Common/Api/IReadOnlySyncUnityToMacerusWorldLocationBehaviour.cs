﻿using Macerus.Api.Behaviors;

namespace Assets.Scripts.Plugins.Features.GameObjects.Common.Api
{
    public interface IReadOnlySyncUnityToMacerusWorldLocationBehaviour
    {
        IWorldLocationBehavior WorldLocationBehavior { get; }
    }
}