using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanBuilder : MonoBehaviour
{
    // Public Methods \\
    public void BuildOcean(GameObject _oceanPrefab, float _oceanHeight, int[,] _heightMap, float _tileSize)
    {
        GameObject oceanGameObject = Instantiate(_oceanPrefab, new Vector3(0, _oceanHeight, 0), Quaternion.identity, transform);

        float xScale = _heightMap.GetLength(0) * _tileSize;
        float zScale = _heightMap.GetLength(1) * _tileSize;

        oceanGameObject.transform.localScale = new Vector3(xScale, 1, zScale);
    }
}