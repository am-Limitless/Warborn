using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    SpawnPoint[] spawnPoints;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        spawnPoints = GetComponentsInChildren<SpawnPoint>();
    }

    public Transform GetSpawnpoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)].transform;
    }
}
