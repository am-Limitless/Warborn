using Photon.Pun;
using System.IO;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;

    // Define a range for the random spawn positions
    [SerializeField] private Vector3 spawnRangeMin = new Vector3(-10, 0, -10);
    [SerializeField] private Vector3 spawnRangeMax = new Vector3(10, 0, 10);

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (PV.IsMine)
        {
            CreateController();
        }
    }

    private void CreateController()
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(spawnRangeMin.x, spawnRangeMax.x),
            Random.Range(spawnRangeMin.y, spawnRangeMax.y),
            Random.Range(spawnRangeMin.z, spawnRangeMax.z)
        );

        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), randomPosition, Quaternion.identity);
    }
}
