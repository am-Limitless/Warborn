using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.IO;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Variables

    private PhotonView PV;

    GameObject controller;

    #endregion

    #region Unity Methods

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

    #endregion

    #region Player Management

    private void CreateController()
    {
        Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();

        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
    }

    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        CreateController();
    }

    #endregion
}
