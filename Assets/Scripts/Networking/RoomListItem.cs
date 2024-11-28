using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomListItem : MonoBehaviour
{
    #region Fields and References

    [Header("UI Components")]
    [SerializeField] TMP_Text infoText;

    public RoomInfo info;

    #endregion

    #region Setup Methods

    public void SetUp(RoomInfo _info)
    {
        info = _info;
        infoText.text = _info.Name;
    }

    #endregion

    #region UI Interaction

    public void OnClick()
    {
        Launcher.Instance.JoinRoom(info);
    }

    #endregion
}
