using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    // Serialized Fields \\
    [Header("Components")]
    [SerializeField] private WorldBuilder worldBuilder;

    [Header("World Settings")]
    [SerializeField] private bool randomSeed = true;
    [SerializeField] private int worldSeed = 0;
    [SerializeField] private int worldWidth = 50;
    [SerializeField] private int worldLength = 50;

    // Private Fields \\
    private World world;

    // Private Methods \\
    private void Start()
    {
        MakeWorld();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MakeWorld();
        }
    }

    [ContextMenu("Make World")]
    private void MakeWorld()
    {
        int seed = worldSeed;

        if (randomSeed)
        {
            seed = Random.Range(int.MinValue, int.MaxValue);
        }

        world = ProceduralWorldGenerator.GenerateWorld(seed, worldWidth, worldLength);
        worldBuilder.BuildWorld(world);
    }
}