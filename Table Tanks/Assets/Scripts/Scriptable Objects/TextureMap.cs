using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TextureMap", menuName = "ScriptableObjects/New Texture Map")]
public class TextureMap : ScriptableObject
{
    // Serialized Fields \\
    [Header("Texture Settings")]
    [SerializeField] private Texture2D texture;
    [SerializeField] private int pixelPadding = 1;

    [Header("Tiling Settings")]
    [SerializeField] [Tooltip("x")] private int rows = 4;
    [SerializeField] [Tooltip("y")] private int columns = 4;

    [Header("Tiles")]
    [SerializeField] private TileSetXY[] tiles = new TileSetXY[0];

    // Private Fields \\
    private float tileWidth;
    private float tileHeight;

    private float halfTileWidth;
    private float halfTileHeight;

    private float widthMargin;
    private float heightMargin;

    private float doubleHeightMargin;

    // Private Methods \\
    private UVCoordinatePair UVCoordinatePairFromXY(TileXY _uvTile)
    {
        int x = _uvTile.x - 1;
        int y = columns - _uvTile.y;
        float raiseBottomAmount = _uvTile.raiseBottomAmount * (tileHeight - doubleHeightMargin);

        Vector2 offset = new Vector2(halfTileWidth, halfTileHeight);
        Vector2 center = offset + new Vector2(x * tileWidth, y * tileHeight);

        Vector2 topLeft = center + new Vector2(-halfTileWidth + widthMargin, halfTileHeight - heightMargin);
        Vector2 bottomRight = center + new Vector2(halfTileWidth - widthMargin, -halfTileHeight + raiseBottomAmount + heightMargin);

        return new UVCoordinatePair(topLeft, bottomRight);
    }

    private Dictionary<LandType, TileSetUV> GenerateTileUVPairs()
    {
        Dictionary<LandType, TileSetUV> tileSetsUV = new Dictionary<LandType, TileSetUV>();

        int length = tiles.Length;

        for (int i = 0; i < length; i++)
        {
            TileSetXY textureSet = tiles[i];

            UVCoordinatePair capPair = UVCoordinatePairFromXY(textureSet.capUVTile);
            UVCoordinatePair wallTopPair = UVCoordinatePairFromXY(textureSet.wallTopUVTile);
            UVCoordinatePair wallMiddlePair = UVCoordinatePairFromXY(textureSet.wallMiddleUVTile);
            UVCoordinatePair wallBottomPair = UVCoordinatePairFromXY(textureSet.wallBottomUVTile);

            TileSetUV newTile = new TileSetUV(capPair, wallTopPair, wallMiddlePair, wallBottomPair);

            tileSetsUV.Add(textureSet.landType, newTile);
        }

        return tileSetsUV;
    }

    // Public Methods \\
    public TextureMeshUVData GetTextureMapUVData()
    {
        tileWidth = 1f / rows;
        tileHeight = 1f / columns;

        halfTileWidth = tileWidth / 2f;
        halfTileHeight = tileHeight / 2f;

        widthMargin = (float)pixelPadding / texture.width;
        heightMargin = (float)pixelPadding / texture.height;

        doubleHeightMargin = heightMargin * 2f;

        Dictionary<LandType, TileSetUV> tileSetsUV = GenerateTileUVPairs();

        return new TextureMeshUVData(tileSetsUV);
    }

    // Protected Classes \\
    [System.Serializable]
    protected class TileSetXY
    {
        public LandType landType = LandType.grass;

        public TileXY capUVTile = new TileXY();
        public TileXY wallTopUVTile = new TileXY();
        public TileXY wallMiddleUVTile = new TileXY();
        public TileXY wallBottomUVTile = new TileXY();
    }

    [System.Serializable]
    protected class TileXY
    {
        public int x = 0;
        public int y = 0;
        public float raiseBottomAmount = 0f;
    }
}

public class TextureMapUVSet
{

}

public class TileSetUV
{
    public UVCoordinatePair tileCap;
    public UVCoordinatePair tileWallTop;
    public UVCoordinatePair tileWallMiddle;
    public UVCoordinatePair tileWallBottom;

    public TileSetUV(UVCoordinatePair _tileCap, UVCoordinatePair _tileWallTop, UVCoordinatePair _tileWallMiddle, UVCoordinatePair _tileWallBottom)
    {
        tileCap = _tileCap;
        tileWallTop = _tileWallTop;
        tileWallMiddle = _tileWallMiddle;
        tileWallBottom = _tileWallBottom;
    }
}

public class UVCoordinatePair
{
    public Vector2 topLeft;
    public Vector2 bottomRight;

    public UVCoordinatePair(Vector2 _topLeft, Vector2 _bottomRight)
    {
        topLeft = _topLeft;
        bottomRight = _bottomRight;
    }
}

public class TextureMeshUVData
{
    // Private Fields \\
    private Dictionary<LandType, TileSetUV> tileSets = new Dictionary<LandType, TileSetUV>();

    // Public Methods \\
    public TextureMeshUVData(Dictionary<LandType, TileSetUV> _tileSets)
    {
        tileSets = _tileSets;
    }

    public UVCoordinatePair GetTileUVPair(LandType _landType, FaceType _faceType)
    {
        TileSetUV uvPairs;

        if (tileSets.TryGetValue(_landType, out uvPairs))
        {
            switch (_faceType)
            {
                case FaceType.cap:
                    return uvPairs.tileCap;

                case FaceType.wall_top:
                    return uvPairs.tileWallTop;

                case FaceType.wall_middle:
                    return uvPairs.tileWallMiddle;

                case FaceType.wall_bottom:
                    return uvPairs.tileWallBottom;
            }
        }

        return new UVCoordinatePair(Vector2.zero, Vector2.zero);
    }
}

public enum FaceType
{
    cap,
    wall_top,
    wall_middle,
    wall_bottom
}