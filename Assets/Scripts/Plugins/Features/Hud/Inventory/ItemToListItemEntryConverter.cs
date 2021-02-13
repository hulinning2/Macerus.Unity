﻿using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Plugins.Features.GameObjects.Common.Api;
using Assets.Scripts.Plugins.Features.Hud.Inventory.Api;
using Assets.Scripts.Unity.Resources.Prefabs;

using ProjectXyz.Api.GameObjects;

namespace Assets.Scripts.Plugins.Features.Hud.Inventory
{
    public sealed class ItemToListItemEntryConverter : IItemToListItemEntryConverter
    {
        private readonly IPrefabCreator _prefabCreator;
        private readonly IHasGameObjectBehaviourStitcher _hasGameObjectBehaviourStitcher;
        private readonly IDragInventoryListItemBehaviourStitcher _dragInventoryListItemBehaviourStitcher;
        private readonly IReadOnlyCollection<IInventoryListItemMutator> _inventoryListItemMutators;

        public ItemToListItemEntryConverter(
            IPrefabCreator prefabCreator,
            IHasGameObjectBehaviourStitcher hasGameObjectBehaviourStitcher,
            IDragInventoryListItemBehaviourStitcher dragInventoryListItemBehaviourStitcher,
            IEnumerable<IInventoryListItemMutator> inventoryListItemMutators)
        {
            _prefabCreator = prefabCreator;
            _hasGameObjectBehaviourStitcher = hasGameObjectBehaviourStitcher;
            _dragInventoryListItemBehaviourStitcher = dragInventoryListItemBehaviourStitcher;
            _inventoryListItemMutators = inventoryListItemMutators.ToArray();
        }

        public IInventoryListItemPrefab Convert(
            IGameObject item,
            string itemListEntryPrefabResource)
        {
            var itemEntry = _prefabCreator.CreatePrefab<IInventoryListItemPrefab>(itemListEntryPrefabResource);
            itemEntry.GameObject.name = $"Inventory Item: {item}";

            _hasGameObjectBehaviourStitcher.Attach(
                item,
                itemEntry.GameObject);
            _dragInventoryListItemBehaviourStitcher.Attach(itemEntry);

            foreach (var mutator in _inventoryListItemMutators)
            {
                mutator.Mutate(
                    itemEntry,
                    item);
            }

            return itemEntry;
        }
    }
}