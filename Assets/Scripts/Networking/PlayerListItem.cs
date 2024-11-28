using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    #region Fields and References

    [Header("UI Components")]
    [SerializeField] TMP_Text text;
    Player player;

    #endregion

    #region Setup Methods

    public void SetUp(Player _player)
    {
        player = _player;
        text.text = _player.NickName;
    }

    #endregion

    #region Photon Callbacks

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }

    #endregion
}
