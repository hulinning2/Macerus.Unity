﻿using Assets.Scripts.Plugins.Features.Hud.Equipment.Api;
using Assets.Scripts.Unity.GameObjects;

using ProjectXyz.Plugins.Features.CommonBehaviors.Api;

namespace Assets.Scripts.Plugins.Features.GameObjects.Actors.UnityBehaviours
{
    public interface IReadonlyHasGuiEquipmentBehaviour
    {
        IEquipmentSlotsFactory EquipmentSlotsFactory { get; }

        IGameObjectManager GameObjectManager { get; }

        IObjectDestroyer ObjectDestroyer { get; }

        IHasEquipmentBehavior HasEquipmentBehavior { get; }

        ICanEquipBehavior CanEquipBehavior { get; }
    }
}