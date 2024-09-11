using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuilder : MonoBehaviour
{
    // Serialized Fields \\
    [Header("Variables")]
    [SerializeField] private float oceanTileHeight = 0.125f;
    [SerializeField] private float tileSize = 1f;
    [SerializeField] private float tileHeight = 0.25f;

    [Header("Prefabs")]
    [SerializeField] private GameObject chunkPrefab;

    // Private Fields \\
    private World world;
    private GameObject chunkObject;

    // Public Methods \\
    public void BuildWorld(World _world)
    {
        world = _world;

        if (chunkObject)
        {
            Destroy(chunkObject);
        }

        Chunk newChunk = new Chunk();

        newChunk.heightMap = _world.heightMap;
        newChunk.landMap = _world.landMap;
        newChunk.vegitationMap = _world.vegitationMap;

        chunkObject = Instantiate(chunkPrefab, Vector3.zero, Quaternion.identity, transform);
        ChunkBuilder chunkBuilder = chunkObject.GetComponent<ChunkBuilder>();

        chunkBuilder.BuildChunk(newChunk, oceanTileHeight, tileSize, tileHeight);
    }
}