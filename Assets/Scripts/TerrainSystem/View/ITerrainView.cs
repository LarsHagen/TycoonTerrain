using System.Collections.Generic;

namespace TerrainSystem.View
{
    public interface ITerrainView
    {
        IReadOnlyDictionary<(int chunkStartX, int chunkStartZ), ChunkView> TerrainChunks { get; }

        void Initialize(TerrainConfig terrainConfig, Terrain terrain);
        void OnTileUpdated(TerrainTile tile);
        void RedrawEntireMap();
    }
}