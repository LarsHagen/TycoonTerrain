using Assets.Scripts.TerrainSystem;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainSystem.View
{
    public class TerrainView : MonoBehaviour
    {
        public Material TerrainMaterial;

        public IReadOnlyDictionary<TerrainChunk, ChunkView> TerrainChunks => _terrainChunks;
        private Dictionary<TerrainChunk, ChunkView> _terrainChunks = new Dictionary<TerrainChunk,ChunkView>();
        private TerrainConfig _terrainConfig;

        public void Initialize(TerrainConfig terrainConfig, Terrain terrain)
        {
            _terrainConfig = terrainConfig;

            foreach (var chunk in terrain.Chunks)
                UpdateChunkMesh(chunk);
        }

        public void UpdateChunkMesh(TerrainChunk chunk)
        {
            ChunkView chunkView = GetOrCreateChunkView(chunk);
            chunkView.UpdateMesh();
        }

        private ChunkView GetOrCreateChunkView(TerrainChunk chunk)
        {
            ChunkView chunkView;
            if (!_terrainChunks.TryGetValue(chunk, out chunkView))
            {
                chunkView = new GameObject("Chunk (" + chunk.ChunkPositionX + "," + chunk.ChunkPositionZ + ")").AddComponent<ChunkView>();
                chunkView.transform.SetParent(transform, false);
                chunkView.transform.localPosition = new Vector3(chunk.ChunkPositionX * _terrainConfig.ChunkSizeX * _terrainConfig.TileSizeX, 0, chunk.ChunkPositionZ * _terrainConfig.ChunkSizeZ * _terrainConfig.TileSizeZ);
                chunkView.Initialize(chunk, _terrainConfig, TerrainMaterial);
                _terrainChunks.Add(chunk, chunkView);
            }

            return chunkView;
        }
    }
}
