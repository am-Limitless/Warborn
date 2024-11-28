using UnityEngine;

public class Menu : MonoBehaviour
{
    #region Variables

    [Header("Menu Settings")]
    public string menuName;
    public bool open;

    #endregion

    #region Menu Control Methods

    public void Open()
    {
        open = true;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        open = false;
        gameObject.SetActive(false);
    }

    #endregion
}
