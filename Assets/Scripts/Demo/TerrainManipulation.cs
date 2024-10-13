using TerrainSystem.View;
using UnityEngine;

namespace Demo
{
    public class TerrainManipulation : MonoBehaviour
    {
        [SerializeField] private MainUIController mainUIController;
        public enum Tool { RaiseTerrain, LowerTerrain }
        public Tool tool;

        private void Start()
        {
            RandomMap();
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                OnMouseClick();
            }
        }

        private void OnMouseClick()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                var terrainView = hit.transform.GetComponentInParent<TerrainView>();
                if (terrainView == null)
                    return;

                var tileHit = ServiceLocator.Instance.terrainController.WorldCoordinateToTile(hit.point);

                if (tool == Tool.RaiseTerrain)
                    tileHit.tile.IncreaseCorner(tileHit.closestCorner);
                else
                    tileHit.tile.LowerCorner(tileHit.closestCorner);

                terrainView.UpdateChunkMesh(tileHit.chunk);
            }
        }

        public void RandomMap()
        {
            var noiseScale = 15;
            var noiseFrequency = 0.01f;
            var terrainConfig = ServiceLocator.Instance.terrainConfig;

            for (int chunkX = 0; chunkX < terrainConfig.NumChunksX; chunkX++)
            {
                for (int chunkZ = 0;chunkZ < terrainConfig.NumChunksZ; chunkZ++)
                {
                    var chunk = ServiceLocator.Instance.terrainController.Terrain.Chunks[chunkX, chunkZ];

                    for (int tileX = 0; tileX < terrainConfig.ChunkSizeX; tileX++)
                    {
                        for (int tileZ = 0; tileZ < terrainConfig.ChunkSizeZ; tileZ++)
                        {
                            var worldX = (chunkX * terrainConfig.ChunkSizeX + tileX) * terrainConfig.TileSizeX;
                            var worldZ = (chunkZ * terrainConfig.ChunkSizeZ + tileZ) * terrainConfig.TileSizeZ;

                            var heightSE = Mathf.PerlinNoise(worldX * noiseFrequency, worldZ * noiseFrequency) * noiseScale;
                            var heightNE = Mathf.PerlinNoise(worldX * noiseFrequency, (worldZ + terrainConfig.TileSizeZ) * noiseFrequency) * noiseScale;
                            var heightNW = Mathf.PerlinNoise((worldX + terrainConfig.TileSizeX) * noiseFrequency, (worldZ + terrainConfig.TileSizeZ) * noiseFrequency) * noiseScale;
                            var heightSW = Mathf.PerlinNoise((worldX + terrainConfig.TileSizeX) * noiseFrequency, worldZ * noiseFrequency) * noiseScale;

                            chunk.Tiles[tileX, tileZ].SetCorner(TerrainSystem.TerrainTile.TileCornerDirections.SW, Mathf.RoundToInt(heightSW));
                            chunk.Tiles[tileX, tileZ].SetCorner(TerrainSystem.TerrainTile.TileCornerDirections.NW, Mathf.RoundToInt(heightNW));
                            chunk.Tiles[tileX, tileZ].SetCorner(TerrainSystem.TerrainTile.TileCornerDirections.NE, Mathf.RoundToInt(heightNE));
                            chunk.Tiles[tileX, tileZ].SetCorner(TerrainSystem.TerrainTile.TileCornerDirections.SE, Mathf.RoundToInt(heightSE));
                        }
                    }

                    ServiceLocator.Instance.terrainView.UpdateChunkMesh(chunk);
                }
            }
        }
    }
}