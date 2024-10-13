﻿using Assets.Scripts.TerrainSystem;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainSystem.View
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class ChunkView : MonoBehaviour
    {
        private TerrainMeshData _terrainMeshData;
        private TerrainConfig _config;
        private TerrainChunk _chunk;

        private MeshFilter _filter;
        private MeshRenderer _meshRenderer;
        private MeshCollider _meshCollider;

        internal void Initialize(TerrainChunk chunk, TerrainConfig config, Material terrainMaterial)
        {
            _config = config;
            _chunk = chunk;

            _terrainMeshData = new();

            _filter = GetComponent<MeshFilter>();
            _meshRenderer = GetComponent<MeshRenderer>();
            _meshCollider = GetComponent<MeshCollider>();

            _meshRenderer.sharedMaterial = terrainMaterial;
        }

        public void UpdateMesh()
        {
            _terrainMeshData.Clear();

            for (int x = 0; x < _config.ChunkSizeX; x++)
            {
                for (int z = 0; z < _config.ChunkSizeZ; z++)
                {
                    var tile = _chunk.Tiles[x, z];

                    var worldX = x * _config.TileSizeX;
                    var worldZ = z * _config.TileSizeZ;

                    var posSEBase = new Vector3(worldX, 0, worldZ);
                    var posNEBase = new Vector3(worldX, 0, worldZ + _config.TileSizeZ);
                    var posNWBase = new Vector3(worldX + _config.TileSizeX, 0, worldZ + _config.TileSizeZ);
                    var posSWBase = new Vector3(worldX + _config.TileSizeX, 0, worldZ);

                    var posSE = new Vector3(posSEBase.x, tile.CornerHeights[TerrainTile.TileCornerDirections.SE] * _config.TileStepSize, posSEBase.z);
                    var posNE = new Vector3(posNEBase.x, tile.CornerHeights[TerrainTile.TileCornerDirections.NE] * _config.TileStepSize, posNEBase.z);
                    var posNW = new Vector3(posNWBase.x, tile.CornerHeights[TerrainTile.TileCornerDirections.NW] * _config.TileStepSize, posNWBase.z);
                    var posSW = new Vector3(posSWBase.x, tile.CornerHeights[TerrainTile.TileCornerDirections.SW] * _config.TileStepSize, posSWBase.z);

                    //Add 4 sides
                    _terrainMeshData.AddQuad(posSE, posSW, posSWBase, posSEBase, tile.Sides, tile.Sides, tile.Sides, tile.Sides);
                    _terrainMeshData.AddQuad(posNE, posSE, posSEBase, posNEBase, tile.Sides, tile.Sides, tile.Sides, tile.Sides);
                    _terrainMeshData.AddQuad(posNW, posNE, posNEBase, posNWBase, tile.Sides, tile.Sides, tile.Sides, tile.Sides);
                    _terrainMeshData.AddQuad(posSW, posNW, posNWBase, posSWBase, tile.Sides, tile.Sides, tile.Sides, tile.Sides);

                    //Add top face, make sure it has the correct "fold" depending on what corner is heighest
                    if (posSE.y > posNE.y &&
                        posSE.y > posSW.y)
                    {
                        _terrainMeshData.AddQuad(posSW, posSE, posNE, posNW, tile.Surface, tile.Surface, tile.Surface, tile.Surface);
                        continue;
                    }
                    if (posNW.y > posNE.y &&
                        posNW.y > posSW.y)
                    {
                        _terrainMeshData.AddQuad(posSW, posSE, posNE, posNW, tile.Surface, tile.Surface, tile.Surface, tile.Surface);
                        continue;
                    }
                    _terrainMeshData.AddQuad(posSE, posNE, posNW, posSW, tile.Surface, tile.Surface, tile.Surface, tile.Surface);
                }
            }

            var mesh = _terrainMeshData.Build();
            _filter.sharedMesh = mesh;
            _meshCollider.sharedMesh = mesh;
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
