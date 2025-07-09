using Sans.UI.Menu;
using UnityEngine;

public class MenuInput : MonoBehaviour
{
    MenuController _menu;

    private void Awake()
    {
        _menu = GetComponent<MenuController>();
    }

    private void Update()
    {
        GetMobileInput();
    }

    private void GetMobileInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_menu.GetCurrentMenu == MenuType.Main)
            {
                _menu.OpenMenu(MenuType.Exit);
            }
            else if (_menu.GetCurrentMenu == MenuType.Setting ||
                     _menu.GetCurrentMenu == MenuType.Credit ||
                     _menu.GetCurrentMenu == MenuType.Exit)
            {
                _menu.CloseMenu();
            }
        }
    }
}
