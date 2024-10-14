using TerrainSystem.View;
using UnityEngine;

namespace TerrainSystem
{
    public interface ITerrainController
    {
        Terrain Terrain { get; }
        TerrainConfig Config { get; }
        ITerrainView TerrainView { get; }

        (TerrainTile tile, TerrainTile.TileCornerDirections closestCorner) WorldCoordinateToTile(Vector3 worldPosition);
    }
}