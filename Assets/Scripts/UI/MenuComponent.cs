using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

public abstract class MenuComponent
{
    //-----------------------------------------------------------------
    
    #region  Variables
    //Every Menu Component has a Panel Parent Object with a Menu attached
    public GameObject PanelObj;
    //The Parent menu on Parent object
    protected Menu p_ParentMenu;

    #endregion

    //-----------------------------------------------------------------

    #region Public virtual Methods
    //Shows Parent Panel or it returns if null
    public virtual void ShowComponent(bool show)
    {
        if(PanelObj == null) return;
        PanelObj.SetActive(show);
    }
    #endregion

    //-----------------------------------------------------------------

    #region Public Methods
    // used for assigning actions to button on initiation
    public void SetMethod(Button button, UnityAction action)
    {
        if (button == null)
        {
            return;
        }
        //Remove the existing events
        button.onClick.RemoveAllListeners();
        //Add your new event
        button.onClick.AddListener(action);
    }
    // same use but  for Toggles
    public void SetMethod(Toggle button, UnityAction<bool> action)
    {
        //Remove the existing events
        button.onValueChanged.RemoveAllListeners();
        //Add your new event
        button.onValueChanged.AddListener(action);
    }

    #endregion

    //-----------------------------------------------------------------
}
