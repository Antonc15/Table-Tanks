using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VegitationCollection", menuName = "ScriptableObjects/New Vegitation Collection")]
public class VegitationCollection : ScriptableObject
{
    // Serialized Fields \\
    [SerializeField] private VegitationTypeAndPrefab[] vegitationList = new VegitationTypeAndPrefab[0];

    // Private Fields \\
    private Dictionary<VegitationType, GameObject> vegitationDictionary = new Dictionary<VegitationType, GameObject>();

    // Public Methods \\
    public void Initialize()
    {
        vegitationDictionary.Clear();

        int length = vegitationList.Length;

        for (int i = 0; i < length; i++)
        {
            VegitationTypeAndPrefab vtp = vegitationList[i];
            vegitationDictionary.Add(vtp.type, vtp.prefab);
        }
    }

    public GameObject GetVegitationPrefab(VegitationType _type)
    {
        return vegitationDictionary[_type];
    }

    // Protected Classes \\
    [System.Serializable]
    protected class VegitationTypeAndPrefab
    {
        public VegitationType type;
        public GameObject prefab;
    }
}