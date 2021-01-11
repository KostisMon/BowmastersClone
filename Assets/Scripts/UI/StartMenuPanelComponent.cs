using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class StartMenuPanelComponent : MenuComponent
{

    //-----------------------------------------------------------------

    #region Variables
    private Button m_PlayButton;

    #endregion

    //-----------------------------------------------------------------

    #region Public Methods

    public StartMenuPanelComponent(Menu menu)
    {
        p_ParentMenu = menu;
        PanelObj = menu.gameObject.transform.Find("StartMenuPanelComponent").gameObject;
        RectTransform[] trans = PanelObj.GetComponentsInChildren<RectTransform>();
        for (int i = 0; i < trans.Length; i++)
        {
            if (trans[i].name == "PlayButton")
            {
                m_PlayButton = trans[i].GetComponent<Button>();
                SetMethod(m_PlayButton, PlayGame);
            }
        }
    }

    #endregion
    //-----------------------------------------------------------------

    #region Private Methods
    private void PlayGame()
    {
        GameManager.Instance.SetState(GameState.PlayerAims);
        UiManager.Instance.SetState(UiManager.MenuState.InGameMenu);
        //ShowComponent(false);

    }
    #endregion

    //-----------------------------------------------------------------

}
