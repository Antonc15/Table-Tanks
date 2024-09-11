using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Chunk
{
    public int[,] heightMap;
    public LandType[,] landMap;
    public VegitationType[,] vegitationMap;
}
