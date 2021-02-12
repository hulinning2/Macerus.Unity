using ProjectXyz.Plugins.Features.CommonBehaviors.Api;

namespace Assets.Scripts.Plugins.Features.Hud.Inventory.Api
{
    public interface IItemListFactory
    {
        IItemListPrefab Create(
            string itemListPrefabResource,
            string itemListItemPrefabResource,
            IItemContainerBehavior itemContainerBehavior);
    }
}