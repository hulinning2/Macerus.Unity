﻿using System.Linq;

using Assets.Scripts.Plugins.Features.Inventory.Noesis;
using Assets.Scripts.Plugins.Features.Inventory.Noesis.Resources;

using Autofac;
using Macerus.Plugins.Features.Inventory.Api;
using Macerus.Plugins.Features.Inventory.Api.HoverCards;

using ProjectXyz.Framework.ViewWelding.Api;
using ProjectXyz.Framework.ViewWelding.Api.Welders;

namespace Assets.Scripts.Plugins.Features.Inventory.Autofac
{
    public sealed class InventoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<PlayerInventoryWindowNoesisViewModel>()
                .AsImplementedInterfaces()
                .SingleInstance();
            builder
                .RegisterType<PlayerInventoryWindow>()
                .AsImplementedInterfaces()
                .SingleInstance();
            builder
                .RegisterType<EquipmentSlotToNoesisViewModelConverter>()
                .AsImplementedInterfaces()
                .SingleInstance();
            builder
                .RegisterType<BagSlotToNoesisViewModelConverter>()
                .AsImplementedInterfaces()
                .SingleInstance();
            builder
                .Register(x =>
                {
                    var itemSlotCollectionNoesisViewModel = new ItemSlotCollectionNoesisViewModel(
                        x.Resolve<IEquipmentSlotToNoesisViewModelConverter>(),
                        x.ResolveNamed<IItemSlotCollectionViewModel>("player equipment"),
                        null);
                    var playerInventoryEquipmentView = new InventoryEquipmentView(itemSlotCollectionNoesisViewModel);
                    return playerInventoryEquipmentView;
                })
                .AsImplementedInterfaces()
                .SingleInstance();
            builder
                .Register(x =>
                {
                    var itemSlotCollectionNoesisViewModel = new ItemSlotCollectionNoesisViewModel(
                        x.Resolve<IBagSlotToNoesisViewModelConverter>(),
                        x.ResolveNamed<IItemSlotCollectionViewModel>("player bag"),
                        null);
                    var playerInventoryBagView = new InventoryBagView(itemSlotCollectionNoesisViewModel);
                    return playerInventoryBagView;
                })
                .AsImplementedInterfaces()
                .SingleInstance();
            builder
                .Register(x =>
                {
                    var itemSlotCollectionNoesisViewModel = new EmptyDropZoneNoesisViewModel(
                        x.ResolveNamed<IItemSlotCollectionViewModel>("drop to map"));
                    return itemSlotCollectionNoesisViewModel;
                })
                .AsImplementedInterfaces()
                .SingleInstance();
            builder
                .Register(x =>
                {
                    var itemDragNoesisViewModel = new ItemDragNoesisViewModel(
                        x.Resolve<IBagSlotToNoesisViewModelConverter>(),
                        x.Resolve<IItemDragViewModel>());
                    return itemDragNoesisViewModel;
                })
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
