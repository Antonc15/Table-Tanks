using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegitationBuilder : MonoBehaviour
{
    // Private Fields \\
    private List<GameObject> vegitationObjects = new List<GameObject>();

    // Private Methods \\
    private void ClearVegitationObjects()
    {
        for (int i = 0; i < vegitationObjects.Count; i++)
        {
            Destroy(vegitationObjects[i]);
        }

        vegitationObjects.Clear();
    }

    private void TryBuildVegitation(GameObject _prefab, Vector3 _pos)
    {
        GameObject newVegitationObject = Instantiate(_prefab, _pos, Quaternion.identity, transform);
        vegitationObjects.Add(newVegitationObject);
    }

    // Public Methods \\
    public void BuildVegitation(VegitationCollection _vegitationCollection, VegitationType[,] _vegitationMap, int[,] _heightMap, float _tileSize, float _tileHeight)
    {
        ClearVegitationObjects();
        _vegitationCollection.Initialize();

        int width = _vegitationMap.GetLength(0);
        int length = _vegitationMap.GetLength(1);

        float tileSize = _tileSize;
        float heightInterval = _tileHeight;

        float halfWidth = ((width - 1) / 2f) * tileSize;
        float halfLength = ((length - 1) / 2f) * tileSize;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                VegitationType vegType = _vegitationMap[x, y];

                if (vegType == VegitationType.none) { continue; }

                float posX = (x * tileSize) - halfWidth;
                float posY = _heightMap[x, y] * heightInterval;
                float posZ = (y * tileSize) - halfLength;

                Vector3 pos = new Vector3(posX, posY, posZ);

                GameObject prefab = _vegitationCollection.GetVegitationPrefab(vegType);

                TryBuildVegitation(prefab, pos);
            }
        }
    }
}
