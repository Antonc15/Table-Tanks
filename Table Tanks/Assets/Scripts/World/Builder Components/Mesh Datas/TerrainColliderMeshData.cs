using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainColliderMeshData
{
    // Private Fields \\
    private float tileSize;
    private float heightInterval;

    private float halfWidth;
    private float halfLength;

    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();

    private Dictionary<Vector3Int, int> vertKeys = new Dictionary<Vector3Int, int>();

    int vertCount = 0;

    // Private Methods \\
    private void AddVerticesAndKeys(Vector3[] _verts, Vector3Int[] _keys)
    {
        int length = _verts.Length;

        for (int i = 0; i < length; i++)
        {
            if (vertKeys.ContainsKey(_keys[i])) { continue; }

            vertices.Add(_verts[i]);
            vertKeys.Add(_keys[i], vertCount);

            vertCount++;
        }
    }

    // Public Methods \\
    public TerrainColliderMeshData(int _width, int _length, float _tileSize, float _tileHeight)
    {
        tileSize = _tileSize;
        heightInterval = _tileHeight;

        halfWidth = ((_width - 1) / 2f) * tileSize;
        halfLength = ((_length - 1) / 2f) * tileSize;
    }

    public void AddTile(int _x, int _y, int _height)
    {
        // Tile Positioning \\
        float originX = (_x * tileSize) - halfWidth;
        float originY = _height * heightInterval;
        float originZ = (_y * tileSize) - halfLength;

        Vector3 origin = new Vector3(originX, originY, originZ);

        // Define Vertices and Keys\\
        Vector3[] newVerts = new Vector3[4];

        newVerts[0] = origin + new Vector3(-0.5f, 0, 0.5f) * tileSize;
        newVerts[1] = origin + new Vector3(0.5f, 0, 0.5f) * tileSize;
        newVerts[2] = origin + new Vector3(0.5f, 0, -0.5f) * tileSize;
        newVerts[3] = origin + new Vector3(-0.5f, 0, -0.5f) * tileSize;

        Vector3Int[] newKeys = new Vector3Int[4];

        newKeys[0] = new Vector3Int(_x - 1, _height, _y);
        newKeys[1] = new Vector3Int(_x, _height, _y);
        newKeys[2] = new Vector3Int(_x, _height, _y - 1);
        newKeys[3] = new Vector3Int(_x - 1, _height, _y - 1);

        // Add Vertices and Keys \\
        AddVerticesAndKeys(newVerts, newKeys);

        // Define and Add Triangles \\
        int[] newTris = new int[6];

        newTris[0] = vertKeys[newKeys[0]]; // vert 1
        newTris[1] = vertKeys[newKeys[1]]; // vert 2
        newTris[2] = vertKeys[newKeys[2]]; // vert 3

        newTris[3] = vertKeys[newKeys[2]]; // vert 3
        newTris[4] = vertKeys[newKeys[3]]; // vert 4
        newTris[5] = vertKeys[newKeys[0]]; // vert 1

        triangles.AddRange(newTris);
    }

    public Mesh GetMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "Terrain Collider Mesh";

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        return mesh;
    }
}
