using UnityEngine;

namespace Assets.Scripts.Gui.Hud.Inventory
{
    public interface IItemListBehaviourStitcher
    {
        IReadonlyItemListBehaviour Attach(
            GameObject listControl,
            GameObject listControlContent,
            string itemListEntryPrefabResource);
    }
}