using Assets.Scripts.TerrainSystem;
using System.Collections.Generic;

namespace TerrainSystem.View
{
    public interface ITerrainView
    {
        IReadOnlyDictionary<TerrainChunk, ChunkView> TerrainChunks { get; }

        void Initialize(TerrainConfig terrainConfig, Terrain terrain);
        void UpdateChunkMesh(TerrainChunk chunk);
    }
}