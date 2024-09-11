using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMeshData
{
    // Private Fields \\
    private float tileSize;
    private float tileHeight;

    private float halfWidth;
    private float halfLength;

    private TextureMeshUVData uvData;

    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private List<Vector2> uvs = new List<Vector2>();

    private int vertCount = 0;

    // Private Methods \\
    private void AddWallSegment(int _x, int _y, int _height, int _top, int _bottom, LandType _landType, WallDirection _wallDirection)
    {
        // Tile Positioning \\
        float originX = (_x * tileSize) - halfWidth;
        float originZ = (_y * tileSize) - halfLength;
        float originY = _height * tileHeight;

        Vector3 topOrigin = new Vector3(originX, originY, originZ);
        Vector3 bottomOrigin = new Vector3(originX, originY - tileHeight, originZ);

        // Define Vertices\\
        Vector3[] newVerts = GetWallReferenceVertices(_wallDirection);

        newVerts[0] = (newVerts[0] * tileSize) + topOrigin; // Top-Left
        newVerts[1] = (newVerts[1] * tileSize) + topOrigin; // Top-Right
        newVerts[2] = (newVerts[2] * tileSize) + bottomOrigin; // Bottom-Right
        newVerts[3] = (newVerts[3] * tileSize) + bottomOrigin; // Bottom-Left

        // Define Triangles \\
        int[] newTris = new int[6];

        newTris[0] = vertCount; // vert 1
        newTris[1] = vertCount + 1; // vert 2
        newTris[2] = vertCount + 2; // vert 3

        newTris[3] = vertCount + 2; // vert 3
        newTris[4] = vertCount + 3; // vert 4
        newTris[5] = vertCount; // vert 1

        // Define Wall Layer \\
        FaceType wallLayer = FaceType.wall_middle;

        if (_height <= _bottom + 1)
        {
            wallLayer = FaceType.wall_bottom;
        }

        if (_height >= _top)
        {
            wallLayer = FaceType.wall_top;
        }

        // Define UV Coordinate Pair \\
        UVCoordinatePair uvCoordPair = uvData.GetTileUVPair(_landType, wallLayer);

        Vector2 topLeft = uvCoordPair.topLeft;
        Vector2 bottomRight = uvCoordPair.bottomRight;

        // Define UV's \\
        Vector2[] newUvs = new Vector2[4];

        newUvs[0] = topLeft; // Top-Left
        newUvs[1] = new Vector2(bottomRight.x, topLeft.y); // Top-Right
        newUvs[2] = bottomRight; // Bottom-Right
        newUvs[3] = new Vector2(topLeft.x, bottomRight.y); // Bottom-Left

        // Add Vertices, Triangles, and UV's \\ 
        vertices.AddRange(newVerts);
        triangles.AddRange(newTris);
        uvs.AddRange(newUvs);

        vertCount += 4;
    }

    private Vector3[] GetWallReferenceVertices(WallDirection _wallDirection)
    {
        Vector3[] verts = new Vector3[4];

        switch (_wallDirection)
        {
            case WallDirection.front:
                verts[0] = new Vector3(0.5f, 0, 0.5f); // Top-Left
                verts[1] = new Vector3(-0.5f, 0, 0.5f); // Top-Right
                verts[2] = new Vector3(-0.5f, 0, 0.5f); // Bottom-Right
                verts[3] = new Vector3(0.5f, 0, 0.5f); // Bottom-Left
                break;

            case WallDirection.back:
                verts[0] = new Vector3(-0.5f, 0, -0.5f); // Top-Left
                verts[1] = new Vector3(0.5f, 0, -0.5f); // Top-Right
                verts[2] = new Vector3(0.5f, 0, -0.5f); // Bottom Right
                verts[3] = new Vector3(-0.5f, 0, -0.5f); // Bottom-Left
                break;

            case WallDirection.left:
                verts[0] = new Vector3(-0.5f, 0, 0.5f); // Top-Left
                verts[1] = new Vector3(-0.5f, 0, -0.5f); // Top-Right
                verts[2] = new Vector3(-0.5f, 0, -0.5f); // Bottom-Right
                verts[3] = new Vector3(-0.5f, 0, 0.5f); // Bottom-Left
                break;

            case WallDirection.right:
                verts[0] = new Vector3(0.5f, 0, -0.5f); // Top-Left
                verts[1] = new Vector3(0.5f, 0, 0.5f); // Top-Right
                verts[2] = new Vector3(0.5f, 0, 0.5f); // Bottom-Right
                verts[3] = new Vector3(0.5f, 0, -0.5f); // Bottom-Left
                break;
        }

        return verts;
    }

    // Public Methods \\
    public TerrainMeshData(int _width, int _length, TextureMeshUVData _uvData, float _tileSize, float _tileHeight)
    {
        tileSize = _tileSize;
        tileHeight = _tileHeight;

        halfWidth = ((_width - 1) / 2f) * tileSize;
        halfLength = ((_length - 1) / 2f) * tileSize;

        uvData = _uvData;
    }

    public void AddTileCap(int _x, int _y, int _height, LandType _landType)
    {
        // Tile Positioning \\
        float originX = (_x * tileSize) - halfWidth;
        float originY = _height * tileHeight;
        float originZ = (_y * tileSize) - halfLength;

        Vector3 origin = new Vector3(originX, originY, originZ);

        // Define Vertices \\
        Vector3[] newVerts = new Vector3[4];

        newVerts[0] = origin + new Vector3(-0.5f, 0, 0.5f) * tileSize; // Up-Left
        newVerts[1] = origin + new Vector3(0.5f, 0, 0.5f) * tileSize; // Up-Right
        newVerts[2] = origin + new Vector3(0.5f, 0, -0.5f) * tileSize; // Down-Right
        newVerts[3] = origin + new Vector3(-0.5f, 0, -0.5f) * tileSize; // Down-Left

        // Define Triangles \\
        int[] newTris = new int[6];

        newTris[0] = vertCount; // vert 1
        newTris[1] = vertCount + 1; // vert 2
        newTris[2] = vertCount + 2; // vert 3

        newTris[3] = vertCount + 2; // vert 3
        newTris[4] = vertCount + 3; // vert 4
        newTris[5] = vertCount; // vert 1

        // Define UV Coordinate Pair \\
        UVCoordinatePair uvCoordPair = uvData.GetTileUVPair(_landType, FaceType.cap);

        Vector2 topLeft = uvCoordPair.topLeft;
        Vector2 bottomRight = uvCoordPair.bottomRight;

        // Define UV's \\
        Vector2[] newUvs = new Vector2[4];

        newUvs[0] = topLeft; // Top-Left
        newUvs[1] = new Vector2(bottomRight.x, topLeft.y); // Top-Right
        newUvs[2] = bottomRight; // Bottom-Right
        newUvs[3] = new Vector2(topLeft.x, bottomRight.y); // Bottom-Left

        // Add Vertices, Triangles, and UV's \\ 
        vertices.AddRange(newVerts);
        triangles.AddRange(newTris);
        uvs.AddRange(newUvs);

        vertCount += 4;
    }

    public void AddTileWall(int _x, int _y, int _top, int _bottom, LandType _landType, WallDirection _wallDirection)
    {
        for (int i = _top; i > _bottom; i--)
        {
            AddWallSegment(_x, _y, i, _top, _bottom, _landType, _wallDirection);
        }
    }

    public Mesh GetMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "Terrain Mesh";

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();

        mesh.RecalculateNormals();

        return mesh;
    }
}

public enum WallDirection
{
    front,
    back,
    left,
    right
}