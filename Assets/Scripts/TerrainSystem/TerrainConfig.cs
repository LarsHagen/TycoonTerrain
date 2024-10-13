namespace TerrainSystem
{
    public class TerrainConfig
    {
        public int ChunkSizeX = 16;
        public int ChunkSizeZ = 16;

        public int NumChunksX = 16;
        public int NumChunksZ = 16;

        public float TileSizeX = 2;
        public float TileSizeZ = 2;
        public float TileStepSize = 1;
        public int? LimitCornerDifference = 1;
    }
}
