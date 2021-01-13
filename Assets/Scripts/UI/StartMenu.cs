using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//The Menu containing the Start menu panel component

public class StartMenu : Menu
{
    //-----------------------------------------------------------------

    #region Variables
    private StartMenuPanelComponent m_StartMenuPanelComponent;

    private List<MenuComponent> m_MainMenuPanelComponentList = new List<MenuComponent>();

    #endregion

    //-----------------------------------------------------------------

    #region Public Methods
    //Creating a new start menu panel component and adding it to the list

    public override void Awake()
    {
        base.Awake();
        m_StartMenuPanelComponent = new StartMenuPanelComponent(this);

        m_MainMenuPanelComponentList.Add(m_StartMenuPanelComponent);

    }

    //used for initialization
    public override void Start()
    {
        base.Start();
        ShowStartButtonsPanel(true);

    }

    //base method for hiding panels
    public void HideAllPanels()
    {
        for (int i = 0; i < m_MainMenuPanelComponentList.Count; i++)
        {
            m_MainMenuPanelComponentList[i].ShowComponent(false);
        }
    }

    //base method for showing start panel component
    public void ShowStartButtonsPanel(bool show)
    {
        HideAllPanels();
        m_StartMenuPanelComponent.ShowComponent(show);
    }
    #endregion

    //-----------------------------------------------------------------

}
