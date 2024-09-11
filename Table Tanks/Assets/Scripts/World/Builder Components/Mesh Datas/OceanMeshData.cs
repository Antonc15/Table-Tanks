using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanMeshData
{
    // Private Fields \\
    private float tileSize;

    private Vector2 uvScalar;

    private float halfWidth;
    private float halfLength;

    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private List<Vector2> uvs = new List<Vector2>();

    private Dictionary<Vector2Int, int> vertKeys = new Dictionary<Vector2Int, int>();
    private Dictionary<Vector2Int, int> uvKeys = new Dictionary<Vector2Int, int>();

    int vertCount = 0;
    int uvCount = 0;

    // Private Methods \\
    private void AddVerticesAndKeys(Vector3[] _verts, Vector2Int[] _keys)
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

    private void AddUVsAndKeys(Vector2[] _uvs, Vector2Int[] _keys)
    {
        int length = _uvs.Length;

        for (int i = 0; i < length; i++)
        {
            if (uvKeys.ContainsKey(_keys[i])) { continue; }

            uvs.Add(_uvs[i]);
            uvKeys.Add(_keys[i], uvCount);

            uvCount++;
        }
    }

    // Public Methods \\
    public OceanMeshData(int _width, int _length, float _tileSize)
    {
        tileSize = _tileSize;

        uvScalar = new Vector2(1f / _width, 1f / _length);

        halfWidth = ((_width - 1) / 2f) * tileSize;
        halfLength = ((_length - 1) / 2f) * tileSize;
    }

    public void AddTile(int _x, int _y)
    {
        // Tile Positioning \\
        float originX = (_x * tileSize) - halfWidth;
        float originZ = (_y * tileSize) - halfLength;

        Vector3 origin = new Vector3(originX, 0f, originZ);

        // Define Vertices, UVs, and Keys\\
        Vector3[] newVerts = new Vector3[4];

        newVerts[0] = origin + new Vector3(-0.5f, 0, 0.5f) * tileSize;
        newVerts[1] = origin + new Vector3(0.5f, 0, 0.5f) * tileSize;
        newVerts[2] = origin + new Vector3(0.5f, 0, -0.5f) * tileSize;
        newVerts[3] = origin + new Vector3(-0.5f, 0, -0.5f) * tileSize;

        Vector2[] newUVs = new Vector2[4];

        newUVs[0] = new Vector2(_x, _y + 1) * uvScalar;
        newUVs[1] = new Vector2(_x + 1, _y + 1) * uvScalar;
        newUVs[2] = new Vector2(_x + 1, _y) * uvScalar;
        newUVs[3] = new Vector2(_x, _y) * uvScalar;

        Vector2Int[] newKeys = new Vector2Int[4];

        newKeys[0] = new Vector2Int(_x - 1, _y);
        newKeys[1] = new Vector2Int(_x, _y);
        newKeys[2] = new Vector2Int(_x, _y - 1);
        newKeys[3] = new Vector2Int(_x - 1, _y - 1);

        // Add Vertices and UV's
        AddVerticesAndKeys(newVerts, newKeys);
        AddUVsAndKeys(newUVs, newKeys);

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
        mesh.name = "Ocean Mesh";

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();

        mesh.RecalculateNormals();

        return mesh;
    }
}