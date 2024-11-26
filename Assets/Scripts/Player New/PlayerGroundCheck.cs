using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    #region Variables

    PlayerController playerController;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsPlayerCollider(other))
        {
            playerController.SetGroundedState(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsPlayerCollider(other))
        {
            playerController.SetGroundedState(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!IsPlayerCollider(other))
        {
            playerController.SetGroundedState(true);
        }
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Checks if the collider belongs to the player.
    /// </summary>
    private bool IsPlayerCollider(Collider other)
    {
        return other.gameObject == playerController.gameObject;
    }

    #endregion
}
