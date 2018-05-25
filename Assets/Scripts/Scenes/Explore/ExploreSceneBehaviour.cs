﻿using System.Linq;
using System.Threading;
using Assets.Scripts.Autofac;
using Assets.Scripts.Scenes.Explore.Camera;
using Assets.Scripts.Scenes.Explore.Input;
using Assets.Scripts.Scenes.Explore.Maps;
using Assets.Scripts.Scenes.Explore.Maps.GameObjects;
using Assets.Scripts.Unity.GameObjects;
using Assets.Scripts.Unity.Resources;
using Autofac;
using Macerus.Api.GameObjects;
using ProjectXyz.Game.Interface.Engine;
using ProjectXyz.Game.Interface.GameObjects;
using ProjectXyz.Game.Interface.Mapping;
using ProjectXyz.Shared.Framework;
using UnityEngine;

namespace Assets.Scripts.Scenes.Explore
{
    public sealed class ExploreSceneBehaviour : MonoBehaviour
    {
        private bool _runOnce;
        private IPrefabCreator _prefabCreator;

        private void Start()
        {
            var dependencyContainer = new MacerusContainerBuilder().CreateContainer();

            var gameEngine = dependencyContainer.Resolve<IGameEngine>();
            gameEngine.Start(CancellationToken.None);

            _prefabCreator = dependencyContainer.Resolve<IPrefabCreator>();

            var mapBehaviourStitcher = dependencyContainer.Resolve<IMapBehaviourStitcher>();
            var mapObject = _prefabCreator.Create<GameObject>("Mapping/Prefabs/Map");
            mapObject.transform.parent = gameObject.transform;
            mapObject.name = "Map";
            mapBehaviourStitcher.Attach(mapObject);

            var mapManager = dependencyContainer.Resolve<IMapManager>();
            mapManager.SwitchMap("swamp");

            var guiInputStitcher = dependencyContainer.Resolve<IGuiInputStitcher>();
            guiInputStitcher.Attach(gameObject);

            var gameObjectRepository = dependencyContainer.Resolve<IGameObjectRepository>();
            var playerActor = gameObjectRepository.Load(
                new StringIdentifier("actor"),
                new StringIdentifier("player"));
            var gameObjectManager = dependencyContainer.Resolve<IMutableGameObjectManager>();
            gameObjectManager.MarkForAddition(playerActor);
        }

        private void Update()
        {
            if (_runOnce)
            {
                return;
            }

            var playerObject = gameObject
                .GetComponentsInChildren<IdentifierBehaviour>()
                .SingleOrDefault(x => x.Id.Equals(new StringIdentifier("player")))
                ?.gameObject;
            if (playerObject == null)
            {
                return;
            }

            Debug.Log("Found player on map...");
            var followCamera = _prefabCreator.Create<GameObject>("Mapping/Prefabs/FollowCamera");
            followCamera.transform.parent = gameObject.transform;
            followCamera.name = "FollowCamera";

            var cameraTargetting = followCamera.GetRequiredComponent<ICameraTargetting>();
            cameraTargetting.SetTarget(playerObject.transform);

            _runOnce = true;
        }
    }
}
