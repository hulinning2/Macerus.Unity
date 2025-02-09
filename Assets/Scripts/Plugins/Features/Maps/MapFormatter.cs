﻿using System;
using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Plugins.Features.FogOfWar;
using Assets.Scripts.Plugins.Features.GameObjects.Common;
using Assets.Scripts.Plugins.Features.GameObjects.Common.Api;
using Assets.Scripts.Plugins.Features.Maps.Api;
using Assets.Scripts.Unity.GameObjects;
using Assets.Scripts.Unity.Resources.Prefabs;
using Assets.Scripts.Unity.Threading;

using Macerus.Plugins.Features.Mapping;

using ProjectXyz.Api.Framework;
using ProjectXyz.Api.GameObjects;
using ProjectXyz.Plugins.Features.CommonBehaviors.Api;
using ProjectXyz.Plugins.Features.Mapping;

using UnityEngine;

namespace Assets.Scripts.Plugins.Features.Maps
{
    using ILogger = ProjectXyz.Api.Logging.ILogger;

    public sealed class MapFormatter : IMapFormatter
    {
        // base tile map
        private const int LAYER_GRID_LINES = 10000;
        private const int LAYER_HOVER_SELECT = int.MaxValue;

        // traversable tile map
        private const int LAYER_TRAVERSABLE = 0;

        // targetting
        private const int LAYER_TARGETTING = 20000;

        private readonly ITileLoader _tileLoader;
        private readonly IObjectDestroyer _objectDestroyer;
        private readonly IUnityGameObjectRepository _unityGameObjectRepository;
        private readonly IPrefabCreator _prefabCreator;
        private readonly ILogger _logger;
        private readonly IDispatcher _dispatcher;

        private HashSet<Vector2Int> _traversableTiles;
        private IReadOnlyDictionary<int, HashSet<System.Numerics.Vector2>> _targettedTiles;
        private bool _gridLinesEnabled;
        private IMapPrefab _mapPrefab;
        private int _maximumTileX;
        private int _maximumTileY;
        private int _minimumTileX;
        private int _minimumTileY;
        private Vector3Int? _lastHoverSelectTilePosition;

        public MapFormatter(
            ITileLoader tileLoader,
            IObjectDestroyer objectDestroyer,
            IUnityGameObjectRepository unityGameObjectRepository,
            IPrefabCreator prefabCreator,
            ILogger logger,
            IDispatcher dispatcher)
        {
            _tileLoader = tileLoader;
            _objectDestroyer = objectDestroyer;
            _unityGameObjectRepository = unityGameObjectRepository;
            _prefabCreator = prefabCreator;
            _logger = logger;
            _dispatcher = dispatcher;
        }

        public void FormatMap(
            IMapPrefab mapPrefab,
            IGameObject map)
        {
            _mapPrefab = mapPrefab;
            _logger.Debug($"Formatting map object '{mapPrefab}' for '{map}'...");

            _dispatcher.RunOnMainThread(() =>
            {
                var parentMapObjectTransform = mapPrefab.GameObject.transform;

                int z = 0;
                _minimumTileX = int.MaxValue;
                _maximumTileX = int.MinValue;
                _minimumTileY = int.MaxValue;
                _maximumTileY = int.MinValue;


                mapPrefab.Tilemap.ClearAllTiles();
                foreach (var mapLayer in map?.GetOnly<IMapLayersBehavior>().Layers ?? Enumerable.Empty<IMapLayer>())
                {
                    foreach (var tile in mapLayer.Tiles)
                    {
                        var tileResource = (ITileResourceBehavior)tile.Behaviors.First(x => x is ITileResourceBehavior);
                        var unityTile = _tileLoader.LoadTile(
                            tileResource.TilesetResourcePath,
                            tileResource.SpriteResourceName);
                        var tilePosition = tile.GetOnly<IPositionBehavior>();

                        // must be called from main thread
                        mapPrefab.Tilemap.SetTile(
                            new Vector3Int((int)tilePosition.X, (int)tilePosition.Y, z),
                            unityTile);

                        _maximumTileX = Math.Max(_maximumTileX, (int)tilePosition.X);
                        _minimumTileX = Math.Min(_minimumTileX, (int)tilePosition.X);
                        _maximumTileY = Math.Max(_maximumTileY, (int)tilePosition.Y);
                        _minimumTileY = Math.Min(_minimumTileY, (int)tilePosition.Y);
                    }

                    z++;
                }

                var existingFogOfWar = parentMapObjectTransform.Find("FogOfWar");
                if (map?.TryGetFirst<IHasFogOfWarBehavior>(out var fogOfWarBehavior) == true)
                {
                    var fogOfWarGameObject = _prefabCreator.Create<GameObject>("FogOfWar/FogOfWar");
                    fogOfWarGameObject.name = "FogOfWar";
                    fogOfWarGameObject.transform.SetParent(parentMapObjectTransform);

                    var fogOfWarPrefab = new FogOfWarPrefab(fogOfWarGameObject);

                    var fogWidthInterim = (int)Math.Max(
                        (int)Math.Abs(_minimumTileX),
                        (int)Math.Abs(_maximumTileX));
                    var fogHeightInterim = (int)Math.Max(
                        (int)Math.Abs(_minimumTileY),
                        (int)Math.Abs(_maximumTileY));
                    const float ROUND_UP_HALF_TILES = 1.5f;
                    var fogSize = 2 * (Math.Max(fogWidthInterim, fogHeightInterim) + ROUND_UP_HALF_TILES);

                    fogOfWarPrefab.FogMainTextureTransform.sizeDelta = new Vector2(fogSize, fogSize);
                    fogOfWarPrefab.FogCameraMain.orthographicSize = fogSize / 2;
                }

                if (existingFogOfWar != null)
                {
                    // NOTE: we *MUST* remove the parent reference because
                    // destruction doesn't actually occur until the next engine
                    // tick. as a result, people can accidentally lookup these
                    // objects before the next tick.
                    existingFogOfWar.parent = null;
                    _objectDestroyer.Destroy(existingFogOfWar.gameObject);
                }

                // set these back (if needed) because we just obliterated all the
                // tiles by formatting the tile map
                ToggleGridLines(_gridLinesEnabled, false);
                SetTraversableTiles(_traversableTiles ?? Enumerable.Empty<Vector2Int>(), false);
                SetTargettedTiles(_targettedTiles ?? new Dictionary<int, HashSet<System.Numerics.Vector2>>(), false);

                // must be called from main thread
                mapPrefab.Tilemap.RefreshAllTiles();
                mapPrefab.WalkIndicatorTilemap.RefreshAllTiles();
            });

            _logger.Debug($"Formatted map object '{mapPrefab}' for '{map}'.");
        }

        public void HoverSelectTile(System.Numerics.Vector2? position)
        {
            if (_lastHoverSelectTilePosition.HasValue)
            {
                _mapPrefab.WalkIndicatorTilemap.SetTile(
                    _lastHoverSelectTilePosition.Value,
                    null);
            }

            _lastHoverSelectTilePosition = position == null
                ? (Vector3Int?)null
                : new Vector3Int(
                    (int)position.Value.X,
                    (int)position.Value.Y,
                    LAYER_HOVER_SELECT);

            if (position.HasValue)
            {
                var unityTile = _tileLoader.LoadTile(
                    "mapping/tilesets/",
                    "tile-hover-select-overlay");
                _mapPrefab.WalkIndicatorTilemap.SetTile(
                    _lastHoverSelectTilePosition.Value,
                    unityTile);
            }
        }

        public void SetTraversableTiles(IEnumerable<System.Numerics.Vector2> traversableTiles) =>
            SetTraversableTiles(
                traversableTiles.Select(p => new Vector2Int((int)p.X, (int)p.Y)),
                true);

        public void SetTargettedTiles(IReadOnlyDictionary<int, HashSet<System.Numerics.Vector2>> targettedTiles) =>
            SetTargettedTiles(
                targettedTiles,
                true);

        public void ToggleGridLines(bool enabled) =>
            ToggleGridLines(enabled, true);

        public void RemoveGameObjects(
            IMapPrefab mapPrefab,
            params IIdentifier[] gameObjectIds) => RemoveGameObjects(
                mapPrefab,
                (IEnumerable<IIdentifier>)gameObjectIds);

        public void RemoveGameObjects(
            IMapPrefab mapPrefab,
            IEnumerable<IIdentifier> gameObjectIds)
        {
            var set = new HashSet<IIdentifier>(gameObjectIds);
            var gameObjectLayerObject = mapPrefab.GameObjectLayer;

            foreach (var toRemove in gameObjectLayerObject
                .GetComponentsInChildren<IdentifierBehaviour>() // FIXME: this is a hack to require concrete type
                .Where(x => set.Contains(x.Id))
                .Select(x => x.gameObject))
            {
                _objectDestroyer.Destroy(toRemove);
            }
        }

        public void RemoveGameObjects(IMapPrefab mapPrefab)
        {
            foreach (var toRemove in mapPrefab.GameObjectLayer.GetChildGameObjects().ToArray())
            {
                _objectDestroyer.Destroy(toRemove);
            }
        }

        public void AddGameObjects(
            IMapPrefab mapPrefab,
            params IGameObject[] gameObjects) => AddGameObjects(
            mapPrefab,
            (IEnumerable<IGameObject>)gameObjects);

        public void AddGameObjects(
            IMapPrefab mapPrefab,
            IEnumerable<IGameObject> gameObjects)
        {
            var gameObjectLayerObject = mapPrefab.GameObjectLayer;
            foreach (var gameObject in gameObjects)
            {
                _logger.Debug($"Transforming game object '{gameObject}'...");
                var unityGameObject = _unityGameObjectRepository.Create(gameObject);

                // add the game object to the correct parent
                unityGameObject.transform.parent = gameObjectLayerObject.transform;
                _logger.Debug($"Adding unity game object '{unityGameObject}' to '{gameObjectLayerObject}'...");
            }
        }

        private void SetTraversableTiles(
            IEnumerable<Vector2Int> traversableTiles,
            bool forceRefresh)
        {
            // may not have been loaded yet
            if (_mapPrefab == null || _mapPrefab.WalkIndicatorTilemap == null)
            {
                return;
            }

            _traversableTiles = new HashSet<Vector2Int>(traversableTiles);
            var highlightTile = _tileLoader.LoadTile(
                "mapping/tilesets/",
                "traversable-tile-highlight");

            _dispatcher.RunOnMainThread(() =>
            {
                for (int i = _minimumTileX; i <= _maximumTileX; i++)
                {
                    for (int j = _minimumTileY; j <= _maximumTileY; j++)
                    {
                        var traversable = _traversableTiles.Contains(new Vector2Int(i, j));
                        var unityTile = traversable
                            ? highlightTile
                            : null;
                        var tilePosition = new Vector3Int(i, j, LAYER_TRAVERSABLE);

                        // must be called from main thread
                        _mapPrefab.WalkIndicatorTilemap.SetTile(
                            tilePosition,
                            unityTile);
                    }
                }

                if (forceRefresh)
                {
                    // must be called from main thread
                    _mapPrefab.WalkIndicatorTilemap.RefreshAllTiles();
                }
            });            
        }

        private void SetTargettedTiles(
            IReadOnlyDictionary<int, HashSet<System.Numerics.Vector2>> targettedTiles,
            bool forceRefresh)
        {
            // may not have been loaded yet
            if (_mapPrefab == null || _mapPrefab.WalkIndicatorTilemap == null)
            {
                return;
            }

            _targettedTiles = targettedTiles;

            var targetLookup = new Dictionary<Vector3Int, int>();
            foreach (var entry in targettedTiles
                .Keys
                .SelectMany(set => targettedTiles[set]
                    .Select(targetPosition => Tuple.Create(
                        new Vector3Int(
                            (int)targetPosition.X,
                            (int)targetPosition.Y,
                            LAYER_TARGETTING),
                        set))))
            {
                targetLookup[entry.Item1] = entry.Item2;
            }

            _dispatcher.RunOnMainThread(() =>
            {
                for (int i = _minimumTileX; i <= _maximumTileX; i++)
                {
                    for (int j = _minimumTileY; j <= _maximumTileY; j++)
                    {
                        var tilePosition = new Vector3Int(i, j, LAYER_TARGETTING);
                        var unityTile = targetLookup.TryGetValue(tilePosition, out var set)
                            ? _tileLoader.LoadTile(
                                "mapping/tilesets/",
                                $"targetted-tile-highlight-{set}")
                            : null;

                        // must be called from main thread
                        _mapPrefab.WalkIndicatorTilemap.SetTile(
                            tilePosition,
                            unityTile);
                    }
                }

                if (forceRefresh)
                {
                    // must be called from main thread
                    _mapPrefab.WalkIndicatorTilemap.RefreshAllTiles();
                }
            });
        }

        private void ToggleGridLines(
            bool enabled,
            bool forceRefresh)
        {
            _gridLinesEnabled = enabled;

            // may not have been loaded yet
            if (_mapPrefab == null || _mapPrefab.WalkIndicatorTilemap == null)
            {
                return;
            }

            var tileBorderOverlay = _tileLoader.LoadTile(
                "mapping/tilesets/",
                "tile-border-overlay");

            _dispatcher.RunOnMainThread(() =>
            {
                for (int i = _minimumTileX; i <= _maximumTileX; i++)
                {
                    for (int j = _minimumTileY; j <= _maximumTileY; j++)
                    {
                        var unityTile = enabled
                            ? tileBorderOverlay
                            : null;

                        // must be called from main thread
                        _mapPrefab.WalkIndicatorTilemap.SetTile(
                            new Vector3Int(i, j, LAYER_GRID_LINES),
                            unityTile);
                    }
                }

                if (forceRefresh)
                {
                    // must be called from main thread
                    _mapPrefab.WalkIndicatorTilemap.RefreshAllTiles();
                }
            });
        }
    }
}