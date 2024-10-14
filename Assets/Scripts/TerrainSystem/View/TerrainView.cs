using System.Collections.Generic;
using UnityEngine;

namespace TerrainSystem.View
{
    public class TerrainView : MonoBehaviour, ITerrainView
    {
        [SerializeField] private Material terrainMaterial;

        public IReadOnlyDictionary<(int chunkStartX, int chunkStartZ), ChunkView> TerrainChunks => _terrainChunks;
        private Dictionary<(int chunkStartX, int chunkStartZ), ChunkView> _terrainChunks = new Dictionary<(int chunkStartX, int chunkStartZ), ChunkView>();

        private TerrainConfig _terrainConfig;
        private Terrain _terrain;

        public void Initialize(TerrainConfig terrainConfig, Terrain terrain)
        {
            _terrain = terrain;
            _terrainConfig = terrainConfig;

            //foreach (var chunk in terrain.Chunks)
            for (int x = 0; x < terrainConfig.NumChunksX; x++)
            {
                for (int z = 0; z < terrainConfig.NumChunksZ; z++)
                {
                    UpdateChunkMesh(x, z);
                }
            }
        }

        public void OnTileUpdated(TerrainTile tile)
        {
            int chunkX = tile.WorldPositionX / _terrainConfig.ChunkSizeX;
            int chunkZ = tile.WorldPositionZ / _terrainConfig.ChunkSizeZ;
            GetOrCreateChunkView(chunkX, chunkZ).UpdateMesh();
        }

        private void UpdateChunkMesh(int x, int z)
        {
            ChunkView chunkView = GetOrCreateChunkView(x,z);
            chunkView.UpdateMesh();
        }

        private ChunkView GetOrCreateChunkView(int x, int z)
        {
            var coordinate = (x, z);

            ChunkView chunkView;
            if (!_terrainChunks.TryGetValue(coordinate, out chunkView))
            {
                chunkView = new GameObject("Chunk (" + x + "," + z + ")").AddComponent<ChunkView>();
                chunkView.transform.SetParent(transform, false);
                chunkView.transform.localPosition = new Vector3(x * _terrainConfig.ChunkSizeX * _terrainConfig.TileSizeX, 0, z * _terrainConfig.ChunkSizeZ * _terrainConfig.TileSizeZ);
                chunkView.Initialize(_terrainConfig, _terrain, x, z, terrainMaterial);
                _terrainChunks.Add(coordinate, chunkView);
            }

            return chunkView;
        }

        public void RedrawEntireMap()
        {
            for (int x = 0; x < _terrainConfig.NumChunksX; x++)
            {
                for (int z = 0; z < _terrainConfig.NumChunksZ; z++)
                {
                    UpdateChunkMesh(x, z);
                }
            }
        }
    }
}
