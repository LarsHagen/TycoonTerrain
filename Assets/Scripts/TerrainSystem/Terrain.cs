namespace TerrainSystem
{
    public class Terrain
    {
        public TerrainTile[,] Tiles { get; private set; }
        public Terrain(TerrainConfig config)
        {
            Tiles = new TerrainTile[config.NumChunksX * config.ChunkSizeX, config.NumChunksZ * config.ChunkSizeZ];
            for (int x = 0; x < config.NumChunksX * config.ChunkSizeX; x++)
            {
                for (int z = 0; z < config.NumChunksZ * config.ChunkSizeZ; z++)
                {
                    Tiles[x, z] = new(config, x, z);
                }
            }
        }
    }
}
