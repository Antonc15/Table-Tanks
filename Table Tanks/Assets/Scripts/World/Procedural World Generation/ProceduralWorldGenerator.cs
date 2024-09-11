using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProceduralWorldGenerator
{
    // Public Methods \\
    public static World GenerateWorld(int _seed, int _width, int _length)
    {
        WorldNoise worldNoise = new DefaultWorldNoise(_seed, _width, _length);

        int[,] heightMap = GenerateHeightMap(worldNoise, _width, _length);
        LandType[,] landMap = GenerateLandMap(worldNoise, _width, _length);
        VegitationType[,] vegitationMap = GenerateVegitationMap(worldNoise, _width, _length);

        return CreateWorld(_seed, _width, _length, heightMap, landMap, vegitationMap);
    }

    // Private Methods \\
    private static World CreateWorld(int _seed, int _width, int _length, int[,] _heightMap, LandType[,] _landMap, VegitationType[,] _vegitationMap)
    {
        World newWorld = new World();

        newWorld.seed = _seed;

        newWorld.width = _width;
        newWorld.length = _length;

        newWorld.heightMap = _heightMap;
        newWorld.landMap = _landMap;
        newWorld.vegitationMap = _vegitationMap;

        return newWorld;
    }

    private static int[,] GenerateHeightMap(WorldNoise _worldNoise, int _width, int _length)
    {
        int[,] newHeightMap = new int[_width, _length];

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _length; y++)
            {
                newHeightMap[x, y] = _worldNoise.GetHeight(x, y);
            }
        }

        return newHeightMap;
    }

    private static LandType[,] GenerateLandMap(WorldNoise _worldNoise, int _width, int _length)
    {
        LandType[,] landMap = new LandType[_width, _length];

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _length; y++)
            {
                landMap[x, y] = _worldNoise.GetLandType(x,y);
            }
        }

        return landMap;
    }

    private static VegitationType[,] GenerateVegitationMap(WorldNoise _worldNoise, int _width, int _length)
    {
        VegitationType[,] newVegitationMap = new VegitationType[_width, _length];

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _length; y++)
            {
                newVegitationMap[x, y] = _worldNoise.GetVegitationType(x, y);
            }
        }

        return newVegitationMap;
    }
}