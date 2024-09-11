using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World
{
    public int seed;

    public int width;
    public int length;

    public int[,] heightMap;
    public LandType[,] landMap;
    public VegitationType[,] vegitationMap;
}

public enum LandType
{
    none,
    grass,
    sand,
    cliff
}

public enum VegitationType
{
    none,
    tree1,
    tree2,
    rock1
}