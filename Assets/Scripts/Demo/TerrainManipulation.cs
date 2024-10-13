using System;
using TerrainSystem;
using UnityEngine;

public class TerrainManipulation : MonoBehaviour
{
    [SerializeField] private MainUIController mainUIController;
    public enum Tool { RaiseTerrain, LowerTerrain}
    public Tool tool;

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
            var terrainController = hit.transform.GetComponentInParent<TerrainController>();
            if (terrainController == null)
                return;

            var tileHit = terrainController.WorldCoordinateToTile(hit.point);

            if (tool == Tool.RaiseTerrain)
                tileHit.tile.IncreaseCorner(tileHit.closestCorner);
            else
                tileHit.tile.LowerCorner(tileHit.closestCorner);

            terrainController.terrainView.UpdateChunkMesh(tileHit.chunk);
        }
    }
}
