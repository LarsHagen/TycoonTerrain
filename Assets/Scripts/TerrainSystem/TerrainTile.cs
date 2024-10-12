using System.Collections.Generic;
using UnityEngine;

namespace TerrainSystem
{
    public class TerrainTile
    {
        public Color Surface;
        public Color Sides;

        public enum TileCornerDirections { SW, NW, NE, SE}

        public Dictionary<TileCornerDirections, int> CornerHeights;

        public TerrainTile()
        {
            CornerHeights = new Dictionary<TileCornerDirections, int>();
            CornerHeights[TileCornerDirections.SW] = (int)Random.Range(0,1);
            CornerHeights[TileCornerDirections.NW] = (int)Random.Range(0,1);
            CornerHeights[TileCornerDirections.NE] = (int)Random.Range(0,1);
            CornerHeights[TileCornerDirections.SE] = (int)Random.Range(0,1);

            var surfaceX = Random.Range(2, 6) / 9f;
            var surfaceY = Random.Range(5, 9) / 10f;
            Surface = new Color(surfaceX, surfaceY, 0);

            var sideX = Random.Range(2, 6) / 9f;
            var sideY = Random.Range(5, 9) / 10f;
            Sides = new Color(sideX, sideY, 0);
        }

        public void IncreaseCorner(TileCornerDirections tileCorner)
        {
            CornerHeights[tileCorner]++;

            if (Terrain.terrainConfig.LimitCornerDifference == null)
                return;

            var directionLeft = GetNextDirection(tileCorner, -1);
            var diffLeft = CornerHeights[tileCorner] - CornerHeights[directionLeft];
            if (diffLeft > Terrain.terrainConfig.LimitCornerDifference)
                IncreaseCorner(directionLeft);


            var directionRight = GetNextDirection(tileCorner, 1);
            var diffRight = CornerHeights[tileCorner] - CornerHeights[directionRight];
            if (diffRight > Terrain.terrainConfig.LimitCornerDifference)
                IncreaseCorner(directionRight);
        }

        private TileCornerDirections GetNextDirection(TileCornerDirections current, int direction)
        {
            return (TileCornerDirections)Mathf.RoundToInt(Mathf.Repeat((int)current + direction, 4));
        }

        public void LowerCorner(TileCornerDirections tileCorner)
        {
            CornerHeights[tileCorner]--;

            if (Terrain.terrainConfig.LimitCornerDifference == null)
                return;

            var directionLeft = GetNextDirection(tileCorner, -1);
            var diffLeft = CornerHeights[directionLeft] - CornerHeights[tileCorner];
            if (diffLeft > Terrain.terrainConfig.LimitCornerDifference)
                LowerCorner(directionLeft);


            var directionRight = GetNextDirection(tileCorner, 1);
            var diffRight = CornerHeights[directionRight] - CornerHeights[tileCorner];
            if (diffRight > Terrain.terrainConfig.LimitCornerDifference)
                LowerCorner(directionRight);
        }
    }
}