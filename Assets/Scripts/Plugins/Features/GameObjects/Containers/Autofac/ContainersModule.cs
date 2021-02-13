﻿using Assets.Scripts.Plugins.Features.GameObjects.Static;
using Assets.Scripts.Unity.Resources.Prefabs;

using Autofac;

namespace Assets.Scripts.Plugins.Features.GameObjects.Containers.Autofac
{
    public sealed class ContainersModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<ContainersBehaviorsProvider>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterBuildCallback(c =>
                {
                    var registrar = c.Resolve<IPrefabCreatorRegistrar>();
                    registrar.Register<ILootPrefab>(obj => new LootPrefab(obj));
                });
        }
    }
}
