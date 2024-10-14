using TerrainSystem.View;
using UnityEngine;

namespace TerrainSystem
{
    public class TerrainController : ITerrainController
    {
        public Terrain Terrain { get; private set; }

        public TerrainConfig Config { get; private set; }
        public ITerrainView TerrainView { get; private set; }

        public TerrainController(TerrainConfig config, ITerrainView view)
        {
            Config = config;
            TerrainView = view;

            Terrain = new Terrain(config);
            TerrainView.Initialize(config, Terrain);
        }

        public (TerrainTile tile, TerrainTile.TileCornerDirections closestCorner) WorldCoordinateToTile(Vector3 worldPosition)
        {
            var worldTileX = (int)(worldPosition.x / Config.TileSizeX);
            var worldTileZ = (int)(worldPosition.z / Config.TileSizeZ);

            /*var chunkX = worldTileX / Config.ChunkSizeX;
            var chunkZ = worldTileZ / Config.ChunkSizeZ;

            var chunkTileX = worldTileX - chunkX * Config.ChunkSizeX;
            var chunkTileZ = worldTileZ - chunkZ * Config.ChunkSizeZ;

            var chunk = Terrain.Chunks[chunkX, chunkZ];
            var tile = chunk.Tiles[chunkTileX, chunkTileZ];*/
            var tile = Terrain.Tiles[worldTileX, worldTileZ];


            var localX = worldPosition.x % Config.TileSizeX;
            var localZ = worldPosition.z % Config.TileSizeZ;

            TerrainTile.TileCornerDirections closestCorner;
            if (localX < Config.TileSizeX * 0.5f)
            {
                closestCorner = localZ < Config.TileSizeZ * 0.5f ? TerrainTile.TileCornerDirections.SE : TerrainTile.TileCornerDirections.NE;
            }
            else
            {
                closestCorner = localZ < Config.TileSizeZ * 0.5f ? TerrainTile.TileCornerDirections.SW : TerrainTile.TileCornerDirections.NW;
            }

            return (tile, closestCorner);
        }
    }
}