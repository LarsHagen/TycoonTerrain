using Assets.Scripts.TerrainSystem;
using UnityEngine;

namespace TerrainSystem
{
    public class Terrain
    {
        public TerrainChunk[,] Chunks { get; private set; }
        public Terrain(TerrainConfig config)
        {
            Chunks = new TerrainChunk[config.NumChunksX, config.NumChunksZ];
            for (int x = 0; x < config.NumChunksX; x++)
            {
                for (int z = 0; z < config.NumChunksZ; z++)
                {
                    Chunks[x, z] = new(config, x, z);
                }
            }
        }
    }
}
