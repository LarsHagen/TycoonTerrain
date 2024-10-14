using System.Collections.Generic;
using UnityEngine;

namespace TerrainSystem
{
    public class TerrainTile
    {
        public int WorldPositionX { get; private set; }
        public int WorldPositionZ { get; private set; }
        public Color Surface { get; private set; }
        public Color Sides { get; private set; }
        private TerrainConfig _config;

        public enum TileCornerDirections { SW, NW, NE, SE}

        public Dictionary<TileCornerDirections, int> CornerHeights;

        public TerrainTile(TerrainConfig config, int worldPositionX, int worldPositionZ)
        {
            CornerHeights = new Dictionary<TileCornerDirections, int>();
            CornerHeights[TileCornerDirections.SW] = 0;
            CornerHeights[TileCornerDirections.NW] = 0;
            CornerHeights[TileCornerDirections.NE] = 0;
            CornerHeights[TileCornerDirections.SE] = 0;

            Surface = TextureAtlasCoordinateToVertexColor(5, 8);
            Sides = TextureAtlasCoordinateToVertexColor(3, 0);

            _config = config;
            WorldPositionX = worldPositionX;
            WorldPositionZ = worldPositionZ;
        }

        public void SetSurfaceMaterial(int textureAtlasX, int textureAtlasY)
        {
            Surface = TextureAtlasCoordinateToVertexColor(textureAtlasX, textureAtlasY);
        }

        public void SetSidesMaterial(int textureAtlasX, int textureAtlasY)
        {
            Sides = TextureAtlasCoordinateToVertexColor(textureAtlasX, textureAtlasY);
        }

        private Color TextureAtlasCoordinateToVertexColor(int textureAtlasX, int textureAtlasY)
        {
            return new Color(textureAtlasX / 9f, textureAtlasY / 10f, 0);
        }

        public void SetCorner(TileCornerDirections tileCorner, int value)
        {
            CornerHeights[tileCorner] = value;
            Debug.LogWarning("TODO: Make sure _config.LimitCornerDifference rule is not broken");
        }

        public void IncreaseCorner(TileCornerDirections tileCorner)
        {
            CornerHeights[tileCorner]++;

            if (_config.LimitCornerDifference == null)
                return;

            var directionLeft = GetNextDirection(tileCorner, -1);
            var diffLeft = CornerHeights[tileCorner] - CornerHeights[directionLeft];
            if (diffLeft > _config.LimitCornerDifference)
                IncreaseCorner(directionLeft);


            var directionRight = GetNextDirection(tileCorner, 1);
            var diffRight = CornerHeights[tileCorner] - CornerHeights[directionRight];
            if (diffRight > _config.LimitCornerDifference)
                IncreaseCorner(directionRight);
        }

        private TileCornerDirections GetNextDirection(TileCornerDirections current, int direction)
        {
            return (TileCornerDirections)Mathf.RoundToInt(Mathf.Repeat((int)current + direction, 4));
        }

        public void LowerCorner(TileCornerDirections tileCorner)
        {
            CornerHeights[tileCorner]--;

            if (_config.LimitCornerDifference == null)
                return;

            var directionLeft = GetNextDirection(tileCorner, -1);
            var diffLeft = CornerHeights[directionLeft] - CornerHeights[tileCorner];
            if (diffLeft > _config.LimitCornerDifference)
                LowerCorner(directionLeft);


            var directionRight = GetNextDirection(tileCorner, 1);
            var diffRight = CornerHeights[directionRight] - CornerHeights[tileCorner];
            if (diffRight > _config.LimitCornerDifference)
                LowerCorner(directionRight);
        }
    }
}