using TerrainSystem;
using UnityEngine;

namespace Assets.Scripts.TerrainSystem
{
    public class TerrainChunk
    {
        public readonly int ChunkPositionX;
        public readonly int ChunkPositionZ;

        public readonly TerrainTile[,] Tiles;

        public TerrainChunk (TerrainConfig config, int chunkPositionX, int chunkPositionZ)
        {
            ChunkPositionX = chunkPositionX;
            ChunkPositionZ = chunkPositionZ;

            Tiles = new TerrainTile[config.ChunkSizeX, config.ChunkSizeZ];
            for (int x = 0; x < config.ChunkSizeX; x++)
            {
                for (int z = 0; z < config.ChunkSizeZ; z++)
                {
                    Tiles[x, z] = new(config);
                }
            }
        }
    }
}
