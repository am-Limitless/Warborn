using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    private static Transform[] spawnPoints;

    private static void InitializeSpawnPoints()
    {
        if (spawnPoints == null)
        {
            GameObject[] spawnPointObjects = GameObject.FindGameObjectsWithTag("SpawnPoint");
            spawnPoints = new Transform[spawnPointObjects.Length];

            for (int i = 0; i < spawnPointObjects.Length; i++)
            {
                spawnPoints[i] = spawnPointObjects[i].transform;
            }
        }
    }

    public static Transform GetRandomSpawnPoint()
    {
        InitializeSpawnPoints();

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points found in the scene!");
            return null;
        }

        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }
}
