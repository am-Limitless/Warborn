using UnityEngine;

public class Menu_Manager : MonoBehaviour
{
    #region Singleton Pattern

    public static Menu_Manager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    #region Menu Management

    [Header("Array of Menus managed by the Menu_Manager")]
    [SerializeField] Menu[] menus;

    public void OpenMenu(string menuName)
    {
        foreach (Menu menu in menus)
        {
            if (menu.menuName == menuName)
            {
                OpenMenu(menu);
            }
            else if (menu.open)
            {
                CloseMenu(menu);
            }
        }
    }

    public void OpenMenu(Menu menu)
    {
        foreach (Menu m in menus)
        {
            if (m.open)
            {
                CloseMenu(m);
            }
        }
        menu.Open();
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }

    #endregion
}
