using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMeshBuilder : MonoBehaviour
{
    // Serialized Field \\
    [Header("Components")]
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;

    // Public Fields \\
    public void BuildTerrainMesh(TextureMap _textureMap, int[,] _heightMap, LandType[,] _landMap, int _depth, float _tileSize, float _tileHeight)
    {
        meshFilter.mesh = GenerateTerrainMesh(_textureMap, _heightMap, _landMap, _depth, _tileSize, _tileHeight);
    }

    // Private Fields \\
    private Mesh GenerateTerrainMesh(TextureMap _textureMap, int[,] _heightMap, LandType[,] _landMap, int _depth, float _tileSize, float _tileHeight)
    {
        int width = _heightMap.GetLength(0);
        int length = _heightMap.GetLength(1);

        TextureMeshUVData uvData = _textureMap.GetTextureMapUVData();

        TerrainMeshData meshData = new TerrainMeshData(width, length, uvData, _tileSize, _tileHeight);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                int height = _heightMap[x, y];

                if(height < _depth) { continue; }

                LandType land = _landMap[x, y];

                int frontHeight = height;
                int backHeight = height;
                int leftHeight = height;
                int rightHeight = height;

                // Assign front, back, left, and right tile heights. If the value is less than Sea-Level it is set to Sea-Level.
                if(y < length - 1)
                {
                    frontHeight = _heightMap[x, y + 1];
                }

                if(y > 0)
                {
                    backHeight = _heightMap[x, y - 1];
                }

                if(x < width - 1)
                {
                    rightHeight = _heightMap[x + 1, y];
                }

                if(x > 0)
                {
                    leftHeight = _heightMap[x - 1, y];
                }

                // Add the top of the tile.
                meshData.AddTileCap(x, y, height, land);

                // Add the walls of the tile
                if(frontHeight < height)
                {
                    meshData.AddTileWall(x, y, height, frontHeight, land, WallDirection.front);
                }

                if(backHeight < height)
                {
                    meshData.AddTileWall(x, y, height, backHeight, land, WallDirection.back);
                }

                if(leftHeight < height)
                {
                    meshData.AddTileWall(x, y, height, leftHeight, land, WallDirection.left);
                }

                if(rightHeight < height)
                {
                    meshData.AddTileWall(x, y, height, rightHeight, land, WallDirection.right);
                }
            }
        }

        return meshData.GetMesh();
    }
}