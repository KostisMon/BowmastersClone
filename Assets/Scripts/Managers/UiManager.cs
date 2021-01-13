using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : Singleton<UiManager>
{

    //-----------------------------------------------------------------

    #region Variables
    // Ui Manager has 3 states
    public enum MenuState
    {
        StartMenu,
        InGameMenu,
        ResultMenu,
        Count
    }

    private MenuState m_CurrentMenuState = MenuState.Count;
    public Dictionary<string, Menu> MenuDictionary = new Dictionary<string, Menu>();
    public StartMenu StartMenu;
    public InGameMenu InGameMenu;
    public ResultMenu ResultMenu;

    #endregion

    //-----------------------------------------------------------------

    #region Public Methods
    //Checks for Menu objects and adds them to the Menu dictionary
    public void Awake()
    {
        Menu[] menus = Object.FindObjectsOfType<Menu>();
        for (int i = 0; i < menus.Length; i++)
        {
            MenuDictionary.Add(menus[i].name, menus[i]);
            menus[i].Show(false);
        }
    }

    //Initializing the Manager by populating the menus
    //Also sets up the starting state and initializes the In Game Menu
    public void Init()
    {
        StartMenu = (StartMenu)MenuDictionary[MenuState.StartMenu.ToString()];
        InGameMenu = (InGameMenu)MenuDictionary[MenuState.InGameMenu.ToString()];
        ResultMenu = (ResultMenu)MenuDictionary[MenuState.ResultMenu.ToString()];
        SetState(MenuState.StartMenu);
        InGameMenu.Init();
    }

    //used for swapping menu states
    public void SetState(MenuState state)
    {
        m_CurrentMenuState = state;
        Show(state.ToString());
    }

    #endregion

    //-----------------------------------------------------------------

    #region Private Methods
    //Showing the desired menu using the dictionary
    private void Show(string menuKey)
    {
        if (MenuDictionary.ContainsKey(menuKey))
        {
            Menu menu = MenuDictionary[menuKey];
            Show(menu);
        }
        else
        {
            Debug.LogError("Key: " + menuKey + " doesn't exist");
        }
    }
    // showing menus using the currentmenu property
    private void Show(Menu menu)
    {
        Menu.CurrentMenu.Show(false);
        Menu.CurrentMenu = menu;
        Menu.CurrentMenu.Show(true);
    }

    #endregion

    //-----------------------------------------------------------------
}
