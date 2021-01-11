using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : Singleton<UiManager>
{

    //-----------------------------------------------------------------

    #region Variables

    public enum MenuState
    {
        StartMenu,
        InGameMenu,
        Count
    }

    private MenuState m_CurrentMenuState = MenuState.Count;
    public Dictionary<string, Menu> MenuDictionary = new Dictionary<string, Menu>();
    public StartMenu StartMenu;
    public InGameMenu InGameMenu;

    #endregion

    //-----------------------------------------------------------------

    #region Public Methods

    public void Awake()
    {
        Menu[] menus = Object.FindObjectsOfType<Menu>();
        for (int i = 0; i < menus.Length; i++)
        {
            MenuDictionary.Add(menus[i].name, menus[i]);
            menus[i].Awake();
            menus[i].Show(false);
        }
    }

    public void Init()
    {
        StartMenu = (StartMenu)MenuDictionary[MenuState.StartMenu.ToString()];
        InGameMenu = (InGameMenu)MenuDictionary[MenuState.InGameMenu.ToString()];
        SetState(MenuState.StartMenu);
        InGameMenu.Init();
    }

    public void SetState(MenuState state)
    {
        m_CurrentMenuState = state;
        Show(state.ToString());
    }

    public float FloatScale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {

        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }

    #endregion

    //-----------------------------------------------------------------

    #region Private Methods

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

    private void Show(Menu menu)
    {
        Menu.CurrentMenu.Show(false);
        Menu.CurrentMenu = menu;
        Menu.CurrentMenu.Show(true);
    }

  

    
    #endregion

    //-----------------------------------------------------------------
}
