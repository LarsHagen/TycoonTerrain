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

                terrainView.OnTileUpdated(tileHit.tile);
            }
        }

        public void RandomMap()
        {
            var noiseScale = 15;
            var noiseFrequency = 0.01f;
            var terrainConfig = ServiceLocator.Instance.terrainConfig;

            var terrain = ServiceLocator.Instance.terrainController.Terrain;

            for (int tileX = 0; tileX < terrainConfig.NumChunksX * terrainConfig.ChunkSizeX; tileX++)
            {
                for (int tileZ = 0; tileZ < terrainConfig.NumChunksZ * terrainConfig.ChunkSizeZ; tileZ++)
                {
                    var heightSE = Mathf.PerlinNoise(tileX * noiseFrequency, tileZ * noiseFrequency) * noiseScale;
                    var heightNE = Mathf.PerlinNoise(tileX * noiseFrequency, (tileZ + 1) * noiseFrequency) * noiseScale;
                    var heightNW = Mathf.PerlinNoise((tileX + 1) * noiseFrequency, (tileZ + 1) * noiseFrequency) * noiseScale;
                    var heightSW = Mathf.PerlinNoise((tileX + 1) * noiseFrequency, tileZ * noiseFrequency) * noiseScale;

                    terrain.Tiles[tileX, tileZ].SetCorner(TerrainSystem.TerrainTile.TileCornerDirections.SW, Mathf.RoundToInt(heightSW));
                    terrain.Tiles[tileX, tileZ].SetCorner(TerrainSystem.TerrainTile.TileCornerDirections.NW, Mathf.RoundToInt(heightNW));
                    terrain.Tiles[tileX, tileZ].SetCorner(TerrainSystem.TerrainTile.TileCornerDirections.NE, Mathf.RoundToInt(heightNE));
                    terrain.Tiles[tileX, tileZ].SetCorner(TerrainSystem.TerrainTile.TileCornerDirections.SE, Mathf.RoundToInt(heightSE));
                }
            }

            ServiceLocator.Instance.terrainView.RedrawEntireMap();
        }
    }
}