using Assets.Scripts.TerrainSystem;
using System.Collections.Generic;
using TerrainSystem.View;
using UnityEngine;

namespace TerrainSystem
{
    public class TerrainController : MonoBehaviour
    {
        public int SizeX;
        public int SizeZ;

        public Terrain Terrain;
        public GameObject HitMarker;

        public TerrainConfig config = new();

        public TerrainView terrainView;

        private void Start()
        {
            Terrain = new Terrain(config);
            terrainView.Initialize(config, Terrain);
        }

        public (TerrainChunk chunk, TerrainTile tile, TerrainTile.TileCornerDirections closestCorner) WorldCoordinateToTile(Vector3 worldPosition)
        {
            var worldTileX = (int)(worldPosition.x / config.TileSizeX);
            var worldTileZ = (int)(worldPosition.z / config.TileSizeZ);

            var chunkX = worldTileX / config.ChunkSizeX;
            var chunkZ = worldTileZ / config.ChunkSizeZ;

            var chunkTileX = worldTileX - chunkX * config.ChunkSizeX;
            var chunkTileZ = worldTileZ - chunkZ * config.ChunkSizeZ;

            var chunk = Terrain.Chunks[chunkX, chunkZ];
            var tile = chunk.Tiles[chunkTileX, chunkTileZ];

            var localX = worldPosition.x % config.TileSizeX;
            var localZ = worldPosition.z % config.TileSizeZ;

            TerrainTile.TileCornerDirections closestCorner;
            if (localX < config.TileSizeX * 0.5f)
            {
                closestCorner = localZ < config.TileSizeZ * 0.5f ? TerrainTile.TileCornerDirections.SE : TerrainTile.TileCornerDirections.NE;
            }
            else
            {
                closestCorner = localZ < config.TileSizeZ * 0.5f ? TerrainTile.TileCornerDirections.SW : TerrainTile.TileCornerDirections.NW;
            }

            return (chunk, tile, closestCorner);
        }
    }
}