﻿using Assets.Scripts.Plugins.Features.GameObjects.Common.Api;
using Assets.Scripts.Plugins.Features.Hud.Api;
using Assets.Scripts.Plugins.Features.Hud.Equipment.Api;
using Assets.Scripts.Plugins.Features.Hud.Inventory.Api;
using Assets.Scripts.Unity.GameObjects;
using Assets.Scripts.Unity.Input;
using Assets.Scripts.Unity.Resources.Prefabs;

using Macerus.Api.Behaviors;
using Macerus.Game.GameObjects;

using ProjectXyz.Api.GameObjects;
using NexusLabs.Contracts;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Plugins.Features.Hud.Equipment
{
    public class DragEquipmentItemBehaviour :
        MonoBehaviour,
        IDragEquipmentItemBehaviour,
        IDragHandler,
        IEndDragHandler
    {
        private IDragItemPrefab _dragObject;

        public GameObject InventoryGameObject { get; set; }

        public IDragItemFactory DragItemFactory { get; set; }

        public IObjectDestroyer ObjectDestroyer { get; set; }

        public IEquipSlotPrefab EquipSlot { get; set; }

        public IDropItemHandler DropItemHandler { get; set; }

        public IGameObjectManager GameObjectManager { get; set; }

        public IMouseInput MouseInput { get; set; }

        public void Start()
        {
            Contract.RequiresNotNull(
                InventoryGameObject,
                $"{nameof(InventoryGameObject)} was not set on '{gameObject}.{this}'.");
            Contract.RequiresNotNull(
                EquipSlot,
                $"{nameof(EquipSlot)} was not set on '{gameObject}.{this}'.");
            Contract.RequiresNotNull(
                DragItemFactory,
                $"{nameof(DragItemFactory)} was not set on '{gameObject}.{this}'.");
            Contract.RequiresNotNull(
                ObjectDestroyer,
                $"{nameof(ObjectDestroyer)} was not set on '{gameObject}.{this}'.");
            Contract.RequiresNotNull(
                DropItemHandler,
                $"{nameof(DropItemHandler)} was not set on '{gameObject}.{this}'.");
            Contract.RequiresNotNull(
                GameObjectManager,
                $"{nameof(GameObjectManager)} was not set on '{gameObject}.{this}'.");
            Contract.RequiresNotNull(
                MouseInput,
                $"{nameof(MouseInput)} was not set on '{gameObject}.{this}'.");
        }

        public void OnDestroy()
        {
            ObjectDestroyer.Destroy(_dragObject?.GameObject);
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if (_dragObject == null)
            {
                _dragObject = DragItemFactory.Create(EquipSlot.ActiveIcon);
                _dragObject.SetParent(InventoryGameObject.transform);
            }

            _dragObject
                .GameObject
                .transform
                .position = MouseInput.Position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var droppedOnCanvas = eventData
                .pointerCurrentRaycast
                .gameObject == null;
            if (droppedOnCanvas)
            {
                TryUnequipAndDropItem(eventData);
            }

            ObjectDestroyer.Destroy(_dragObject.GameObject);
            _dragObject = null;
        }

        private bool TryUnequipAndDropItem(PointerEventData eventData)
        {
            var dropEquipmentSlotBehaviour = eventData
                .pointerDrag
                .GetComponent<IDropEquipmentSlotBehaviour>();
            Contract.RequiresNotNull(
                dropEquipmentSlotBehaviour,
                $"'{eventData.pointerDrag}' does not have " +
                $"'{nameof(IReadOnlyHasGameObject)}' as a component");

            var item = eventData
                .pointerDrag
                .GetComponent<IReadOnlyHasGameObject>()
                ?.GameObject;
            Contract.RequiresNotNull(
                item,
                $"'{dropEquipmentSlotBehaviour}' does not have " +
                $"'{nameof(IReadOnlyHasGameObject)}' as a sibling component " +
                $"with a game object set.");

            var playerLocation = GameObjectManager
                .GetPlayer()
                .GetOnly<IReadOnlyWorldLocationBehavior>();

            return DropItemHandler.TryDropItem(
                playerLocation.X,
                playerLocation.Y,
                item,
                () =>
                {
                    if (!dropEquipmentSlotBehaviour
                        .CanEquipBehavior
                        .TryUnequip(
                            dropEquipmentSlotBehaviour.TargetEquipSlotId,
                            out var canBeEquippedBehavior))
                    {
                        Debug.Log($"Cannot unequip '{item}' to drop.");
                        return false;
                    }

                    return true;
                });
        }
    }
}
