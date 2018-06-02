﻿using Assets.Scripts.Unity.Resources;
using UnityEngine;

namespace Assets.Scripts.Scenes.Explore.Maps
{
    public sealed class MapFactory : IMapFactory
    {
        private readonly IPrefabCreator _prefabCreator;
        private readonly IMapBehaviourStitcher _mapBehaviourStitcher;

        public MapFactory(
            IPrefabCreator prefabCreator,
            IMapBehaviourStitcher mapBehaviourStitcher)
        {
            _prefabCreator = prefabCreator;
            _mapBehaviourStitcher = mapBehaviourStitcher;
        }

        public GameObject CreateMap()
        {
            var mapObject = _prefabCreator.Create<GameObject>("Mapping/Prefabs/Map");
            mapObject.name = "Map";
            _mapBehaviourStitcher.Attach(mapObject);
            return mapObject;
        }
    }
}