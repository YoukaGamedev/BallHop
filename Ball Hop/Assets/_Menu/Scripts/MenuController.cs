using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sans.UI.Menu
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] Menu[] _menus;

        private Hashtable _menuTable = new Hashtable();
        private Stack<Menu> _menuStack = new Stack<Menu>();
        private MenuType _currentMenu;

        // property
        public MenuType GetCurrentMenu => _currentMenu;

        private void Awake()
        {
            RegisterAllMenus();
        }

        #region Public Functions
        public void SwitchMenu(MenuType type)
        {
            CloseMenu();    // Disable the last menu
            OpenMenu(type); // Open desired menu
        }

        public void OpenMenu(MenuType type)
        {
            if (type == MenuType.None) return;
            if (!MenuExist(type))
            {
                Debug.LogWarning($"You are trying to open a Menu {type} that has not been registered.");
                return;
            }

            Menu menu = GetMenu(type);
            menu.gameObject.SetActive(true);
            _menuStack.Push(menu);

            _currentMenu = menu.Type;
        }

        public void CloseMenu()
        {
            if (_menuStack.Count <= 0)
            {
                Debug.LogWarning("MenuController CloseMenu ERROR: No menus in stack!");
                return;
            }
            Menu lastMenuStack = _menuStack.Pop();

            // Disable GameObject
            lastMenuStack.SetDisable();

            if (_menuStack.Count > 0)
                _currentMenu = _menuStack.Peek().Type;
        }
        #endregion

        #region Private Functions
        private void RegisterAllMenus()
        {
            foreach (Menu menu in _menus)
            {
                RegisterMenu(menu);

                // disable menu after register to hash table.
                menu.gameObject.SetActive(false);
            }
            Debug.Log("Successfully registered all menus.");
        }

        private void RegisterMenu(Menu menu)
        {
            if (menu.Type == MenuType.None)
            {
                Debug.LogWarning($"You are trying to register a {menu.Type} type menu that has not allowed.");
                return;
            }

            if (MenuExist(menu.Type))
            {
                Debug.LogWarning($"You are trying to register a Menu {menu.Type} that has already been registered.");
                return;
            }

            _menuTable.Add(menu.Type, menu);
        }

        private Menu GetMenu(MenuType type)
        {
            if (!MenuExist(type)) return null;

            return (Menu)_menuTable[type];
        }

        private bool MenuExist(MenuType type)
        {
            return _menuTable.ContainsKey(type);
        }
        #endregion
    }
}