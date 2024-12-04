using Photon.Pun;
using TMPro;
using UnityEngine;

public class UsernameDisplay : MonoBehaviour
{
    [SerializeField] PhotonView playerPV;
    [SerializeField] TMP_Text text;

    private void Start()
    {
        text.text = playerPV.Owner.NickName;
    }
}
