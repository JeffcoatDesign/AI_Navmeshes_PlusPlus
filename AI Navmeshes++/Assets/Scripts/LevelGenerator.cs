using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class LevelGenerator : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public float spacing = 2f;
    public GameObject wall, player;

    private bool playerSpawned = false;

    private void Start()
    {
        GenerateLevel();

        NavMeshSurface navMesh = GetComponent<NavMeshSurface>();
        navMesh.BuildNavMesh();
    }

    private void GenerateLevel()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (Random.value > 0.7f)
                {
                    Vector3 pos = new Vector3 ((x - width / 2f) * spacing, 0f, (y - height / 2f) * spacing);
                    Instantiate (wall, pos, Quaternion.identity, transform);
                } else if (!playerSpawned)
                {
                    Vector3 pos = new Vector3((x - width / 2f) * spacing, 1f, (y - height / 2f) * spacing);
                    Instantiate (player, pos, Quaternion.identity);
                    playerSpawned = true;
                }
            }
        }
    }
}
