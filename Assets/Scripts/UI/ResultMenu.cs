using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The Menu containing the result panel component
public class ResultMenu : Menu
{
    #region Variables

    private ResultMenuPanelComponent m_ResultMenuPanelComponent;
    private List<MenuComponent> m_ResultMenuPanelComponentList = new List<MenuComponent>();

    #endregion

    //-----------------------------------------------------------------
    #region Public Methods
    //Creating a new resultpanel component and adding it to the list
    public override void Awake()
    {
        base.Awake();
        m_ResultMenuPanelComponent = new ResultMenuPanelComponent(this);

        m_ResultMenuPanelComponentList.Add(m_ResultMenuPanelComponent);

    }

    //Used when Player wins
    public void Won()
    {
        m_ResultMenuPanelComponent.Won();
    }

    //used when Player loses
    public void Lost()
    {
        m_ResultMenuPanelComponent.Lost();
    }
    
    //used to hide the result buttons
    public void HideResultButtons()
    {
        m_ResultMenuPanelComponent.HideResultButtons();
    }

    //used to show the result buttons
    public void ShowResultButtons()
    {
        m_ResultMenuPanelComponent.ShowResultButtons();
    }
    #endregion
}
