using UnityEngine;

public class Menu : MonoBehaviour
{
    [Header("Menu Settings")]
    public string menuName; // Name of the menu
    public bool open; // Status to track if the menu is open or closed

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
}
