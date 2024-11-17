using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text infoText;

    public RoomInfo info;

    public void SetUp(RoomInfo _info)
    {
        info = _info;
        infoText.text = _info.Name;
    }

    public void OnClick()
    {
        Launcher.Instance.JoinRoom(info);
    }
}
