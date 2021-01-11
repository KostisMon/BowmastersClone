using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : Menu
{
    //-----------------------------------------------------------------

    #region Variables
    private StartMenuPanelComponent m_StartMenuPanelComponent;

    private List<MenuComponent> m_MainMenuPanelComponentList = new List<MenuComponent>();

    #endregion

    //-----------------------------------------------------------------

    #region Public Methods
    public override void Awake()
    {
        base.Awake();
        m_StartMenuPanelComponent = new StartMenuPanelComponent(this);

        m_MainMenuPanelComponentList.Add(m_StartMenuPanelComponent);

    }

    public override void Start()
    {
        base.Start();
        ShowStartButtonsPanel(true);

    }

    public void HideAllPanels()
    {
        for (int i = 0; i < m_MainMenuPanelComponentList.Count; i++)
        {
            m_MainMenuPanelComponentList[i].ShowComponent(false);
        }
    }

    public void ShowStartButtonsPanel(bool show)
    {
        HideAllPanels();
        m_StartMenuPanelComponent.ShowComponent(show);
    }
    #endregion

    //-----------------------------------------------------------------

}
