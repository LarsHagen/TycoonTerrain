using UnityEngine;
using UnityEngine.TerrainUtils;

namespace TerrainSystem
{
    public class Terrain
    {
        public static TerrainConfig terrainConfig = new();

        public TerrainTile[,] Tiles { get; private set; }

        public Terrain(int sizeX, int sizeZ)
        {
            terrainConfig.TerrainSizeX = sizeX;
            terrainConfig.TerrainSizeZ = sizeZ;
            Tiles = new TerrainTile[terrainConfig.TerrainSizeX, terrainConfig.TerrainSizeZ];

            for (int x = 0; x < terrainConfig.TerrainSizeX; x++)
            {
                for (int z = 0; z < terrainConfig.TerrainSizeZ; z++)
                {
                    Tiles[x, z] = new TerrainTile();
                }
            }
        }

        public (TerrainTile tile, TerrainTile.TileCornerDirections closestCorner) WorldCoordinateToTile(Vector3 worldPosition)
        {
            var tileX = (int)(worldPosition.x / terrainConfig.TileSizeX);
            var tileZ = (int)(worldPosition.z/ terrainConfig.TileSizeZ);

            var tile = Tiles[tileX, tileZ];
            TerrainTile.TileCornerDirections closestCorner;

            var localX = (worldPosition.x - (tileX * terrainConfig.TileSizeX)) / terrainConfig.TileSizeX;
            var localZ = (worldPosition.z - (tileZ * terrainConfig.TileSizeZ)) / terrainConfig.TileSizeZ;

            if (localX < 0.5f)
            {
                closestCorner = localZ < 0.5f ? TerrainTile.TileCornerDirections.SE : TerrainTile.TileCornerDirections.NE;
            }
            else
            {
                closestCorner = localZ < 0.5f ? TerrainTile.TileCornerDirections.SW : TerrainTile.TileCornerDirections.NW;
            }

            return (tile, closestCorner);
        }
    }
}
