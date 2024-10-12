using System.Collections.Generic;
using UnityEngine;

namespace TerrainSystem
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
    public class TerrainController : MonoBehaviour
    {
        public int SizeX;
        public int SizeZ;

        public Terrain Terrain;
        public GameObject HitMarker;
        public TerrainMeshData TerrainMeshData;

        private void Start()
        {
            Terrain = new Terrain(SizeX, SizeZ);
            TerrainMeshData = new TerrainMeshData();

            UpdateMesh();
        }

        private void OnMouseUpAsButton()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                var tileHit = Terrain.WorldCoordinateToTile(hit.point);
                //tileHit.tile.CornerHeights[tileHit.closestCorner] ++;
                tileHit.tile.IncreaseCorner(tileHit.closestCorner);

                UpdateMesh();
            }
        }

        private void UpdateMesh()
        {
            TerrainMeshData.Clear();

            for (int x = 0; x < Terrain.terrainConfig.TerrainSizeX; x++)
            {
                for (int z = 0; z < Terrain.terrainConfig.TerrainSizeZ; z++)
                {
                    var tile = Terrain.Tiles[x, z];

                    var worldX = x * Terrain.terrainConfig.TileSizeX;
                    var worldZ = z * Terrain.terrainConfig.TileSizeZ;

                    var posSEBase = new Vector3(worldX, 0, worldZ);
                    var posNEBase = new Vector3(worldX, 0, worldZ + Terrain.terrainConfig.TileSizeZ);
                    var posNWBase = new Vector3(worldX + Terrain.terrainConfig.TileSizeX, 0, worldZ + Terrain.terrainConfig.TileSizeZ);
                    var posSWBase = new Vector3(worldX + Terrain.terrainConfig.TileSizeX, 0, worldZ);

                    var posSE = new Vector3(posSEBase.x, tile.CornerHeights[TerrainTile.TileCornerDirections.SE] * Terrain.terrainConfig.TileStepSize, posSEBase.z);
                    var posNE = new Vector3(posNEBase.x, tile.CornerHeights[TerrainTile.TileCornerDirections.NE] * Terrain.terrainConfig.TileStepSize, posNEBase.z);
                    var posNW = new Vector3(posNWBase.x, tile.CornerHeights[TerrainTile.TileCornerDirections.NW] * Terrain.terrainConfig.TileStepSize, posNWBase.z);
                    var posSW = new Vector3(posSWBase.x, tile.CornerHeights[TerrainTile.TileCornerDirections.SW] * Terrain.terrainConfig.TileStepSize, posSWBase.z);

                    //Add 4 sides
                    TerrainMeshData.AddQuad(posSE, posSW, posSWBase, posSEBase, tile.Sides, tile.Sides, tile.Sides, tile.Sides);
                    TerrainMeshData.AddQuad(posNE, posSE, posSEBase, posNEBase, tile.Sides, tile.Sides, tile.Sides,tile.Sides);
                    TerrainMeshData.AddQuad(posNW, posNE, posNEBase, posNWBase, tile.Sides, tile.Sides, tile.Sides,tile.Sides);
                    TerrainMeshData.AddQuad(posSW, posNW, posNWBase, posSWBase, tile.Sides, tile.Sides, tile.Sides,tile.Sides);

                    //Add top face, make sure it has the correct "fold" depending on what corner is heighest
                    if (posSE.y > posNE.y &&
                        posSE.y > posSW.y)
                    {
                        TerrainMeshData.AddQuad(posSW, posSE, posNE, posNW, tile.Surface, tile.Surface, tile.Surface, tile.Surface);
                        continue;
                    }
                    if (posNW.y > posNE.y &&
                        posNW.y > posSW.y)
                    {
                        TerrainMeshData.AddQuad(posSW, posSE, posNE, posNW, tile.Surface, tile.Surface, tile.Surface, tile.Surface);
                        continue;
                    }
                    TerrainMeshData.AddQuad(posSE, posNE, posNW, posSW, tile.Surface, tile.Surface, tile.Surface, tile.Surface);
                }
            }

            var mesh = TerrainMeshData.Build();
            GetComponent<MeshFilter>().sharedMesh = mesh;
            GetComponent<MeshCollider>().sharedMesh = mesh;
        }
    }

    public class TerrainMeshData
    {
        public List<Vector3> verticies = new();
        public List<Color> colors = new();
        public List<int> triangles = new();

        public void Clear()
        {
            verticies.Clear();
            colors.Clear();
            triangles.Clear();
        }

        public void AddTriangle(Vector3 posA, Vector3 posB, Vector3 posC, Color colA, Color colB, Color colC)
        {
            var startVertexCount = verticies.Count;

            verticies.Add(posA);
            verticies.Add(posB);
            verticies.Add(posC);

            colors.Add(colA);
            colors.Add(colB);
            colors.Add(colC);

            triangles.Add(startVertexCount);
            triangles.Add(startVertexCount + 1);
            triangles.Add(startVertexCount + 2);
        }

        public void AddQuad(Vector3 posA, Vector3 posB, Vector3 posC, Vector3 posD, Color colA, Color colB, Color colC, Color colD)
        {
            AddTriangle(posA, posB, posC, colA, colB, colC);
            AddTriangle(posC, posD, posA, colC, colD, colA);
        }

        public Mesh Build()
        {
            var mesh = new Mesh()
            {
                name = "TerrainMesh",
                vertices = verticies.ToArray(),
                colors = colors.ToArray(),
                triangles = triangles.ToArray(),
            };

            mesh.RecalculateNormals();

            return mesh;
        }
    }
}