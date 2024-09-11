using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkBuilder : MonoBehaviour
{
    // Serialized Fields \\
    [Header("Components")]
    [SerializeField] private TerrainMeshBuilder terrainMeshBuilder;
    [SerializeField] private TerrainColliderBuilder terrainColliderBuilder;
    [SerializeField] private VegitationBuilder vegitationBuilder;
    [SerializeField] private OceanBuilder oceanBuilder;

    [Header("Variables")]
    [SerializeField] private int terrainMeshDepth = -7;
    [SerializeField] private GameObject oceanTilePrefab;

    [Header("Scriptable Objects")]
    [SerializeField] private TextureMap terrainMeshTextureMap;
    [SerializeField] private VegitationCollection vegitationCollection;

    // Private Fields \\
    private Chunk chunk;

    // Public Methods \\
    public void BuildChunk(Chunk _chunk, float _oceanTileHeight, float _tileSize, float _tileHeight)
    {
        chunk = _chunk;

        terrainMeshBuilder.BuildTerrainMesh(terrainMeshTextureMap, chunk.heightMap, chunk.landMap, terrainMeshDepth, _tileSize, _tileHeight);
        terrainColliderBuilder.BuildTerrainCollider(chunk.heightMap, _tileSize, _tileHeight);
        vegitationBuilder.BuildVegitation(vegitationCollection, chunk.vegitationMap, chunk.heightMap, _tileSize, _tileHeight);
        oceanBuilder.BuildOcean(oceanTilePrefab, _oceanTileHeight, chunk.heightMap, _tileSize);
    }
}