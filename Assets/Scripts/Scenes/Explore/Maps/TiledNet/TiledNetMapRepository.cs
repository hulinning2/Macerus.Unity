﻿using Assets.Scripts.TiledNet;
using ProjectXyz.Api.Framework;
using ProjectXyz.Framework.Interface.Collections;
using ProjectXyz.Game.Interface.Mapping;

namespace Assets.Scripts.Scenes.Explore.Maps.TiledNet
{
    public sealed class TiledNetMapRepository : IMapRepository
    {
        private readonly ITiledNetToMapConverter _tiledNetToMapConverter;
        private readonly ITiledMapLoader _tiledMapLoader;
        private readonly ICache<IIdentifier, IMap> _mapCache;

        public TiledNetMapRepository(
            ITiledNetToMapConverter tiledNetToMapConverter,
            ITiledMapLoader tiledMapLoader,
            ICache<IIdentifier, IMap> mapCache)
        {
            _tiledNetToMapConverter = tiledNetToMapConverter;
            _tiledMapLoader = tiledMapLoader;
            _mapCache = mapCache;
        }

        public IMap LoadMap(IIdentifier mapId)
        {
            IMap cached;
            if (_mapCache.TryGetValue(mapId, out cached))
            {
                return cached;
            }

            var tiledMap = _tiledMapLoader.LoadMap(mapId);

            var map = _tiledNetToMapConverter.Convert(
                mapId,
                tiledMap);
            _mapCache.AddOrUpdate(mapId, map);

            return map;
        }
    }
}