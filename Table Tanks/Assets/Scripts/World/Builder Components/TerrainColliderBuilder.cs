using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainColliderBuilder : MonoBehaviour
{
    // Serialized Fields \\
    [Header("Components")]
    [SerializeField] private MeshCollider meshCollider;

    // Public Methods \\
    public void BuildTerrainCollider(int[,] _heightMap, float _tileSize, float _tileHeight)
    {
        meshCollider.sharedMesh = GenerateColliderMesh(_heightMap, _tileSize, _tileHeight);
    }

    // Private Methods \\
    private Mesh GenerateColliderMesh(int[,] _heightMap, float _tileSize, float _tileHeight)
    {
        int width = _heightMap.GetLength(0);
        int length = _heightMap.GetLength(1);

        TerrainColliderMeshData meshData = new TerrainColliderMeshData(width, length, _tileSize, _tileHeight);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                int height = _heightMap[x, y];

                if(height <= 0) { continue; }

                meshData.AddTile(x, y, height);
            }
        }

        return meshData.GetMesh();
    }
}
