using ProjectXyz.Api.Framework;
using ProjectXyz.Plugins.Features.CommonBehaviors.Api;

namespace Assets.Scripts.Scenes.Explore.Gui.Hud.Equipment
{
    public interface IIconEquipmentSlotBehaviour : IReadOnlyIconEquipmentSlotBehaviour
    {
        new ICanEquipBehavior CanEquipBehavior { get; set; }

        new IIdentifier TargetEquipSlotId { get; set; }
    }
}