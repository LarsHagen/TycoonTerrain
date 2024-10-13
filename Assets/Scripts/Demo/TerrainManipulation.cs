using TerrainSystem.View;
using UnityEngine;

namespace Demo
{
    public class TerrainManipulation : MonoBehaviour
    {
        [SerializeField] private MainUIController mainUIController;
        public enum Tool { RaiseTerrain, LowerTerrain }
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
    }
}