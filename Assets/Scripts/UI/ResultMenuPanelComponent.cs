using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Menu Component used for the end result of the game
public class ResultMenuPanelComponent : MenuComponent
{
    //-----------------------------------------------------------------

    #region Variables
    private Button m_RePlayButton;
    private GameObject m_YouWon, m_YouLost;
    private Button m_ExitButton;

    #endregion

    //-----------------------------------------------------------------

    #region Public Methods
    // Constructor, searching and populating variables
    // Plus setting methods for buttons
    public ResultMenuPanelComponent(Menu menu)
    {
        p_ParentMenu = menu;
        PanelObj = menu.gameObject.transform.Find("ResultMenuPanelComponent").gameObject;
        RectTransform[] trans = PanelObj.GetComponentsInChildren<RectTransform>();
        for (int i = 0; i < trans.Length; i++)
        {
            if (trans[i].name == "RePlayButton")
            {
                m_RePlayButton = trans[i].GetComponent<Button>();
                SetMethod(m_RePlayButton, RePlayGame);

            }
            if (trans[i].name == "ExitButton")
            {
                m_ExitButton = trans[i].GetComponent<Button>();
                SetMethod(m_ExitButton, RePlayGame);

            }
            if (trans[i].name == "YouWon")
            {
                m_YouWon = trans[i].gameObject;
            }
            if (trans[i].name == "YouLost")
            {
                m_YouLost = trans[i].gameObject;

            }

        }
        Init();
    }

    #endregion

    //-----------------------------------------------------------------

    #region Private Methods
    // used by Replay button, initialising managers and
    // setting up correct states
    private void RePlayGame()
    {
        Init();
        InputManager.Instance.Init();
        GameManager.Instance.ClearProjectiles();
        UiManager.Instance.SetState(UiManager.MenuState.StartMenu);
        AudioManager.Instance.Play();
        GameManager.Instance.SetState(GameState.MainMenu);
    }

    //used by Exit button, quits app
    private void ExitGame()
    {
        Application.Quit();
    }

    //Called when Player Won
    public void Won()
    {
        HideWonLost();
        m_YouWon.SetActive(true);
    }

    //Called when Player Lose
    public void Lost()
    {
        HideWonLost();
        m_YouLost.SetActive(true);
    }

    //Initiales and hides all UIs
    public void Init()
    {
        HideResultButtons();
        HideWonLost();
    }

    //Hides the Win/Lose icons
    public void HideWonLost()
    {
        m_YouLost.SetActive(false);
        m_YouWon.SetActive(false);
    }

    //Showing buttons when win/lose animation is finished
    public void ShowResultButtons()
    {
        m_ExitButton.gameObject.SetActive(true);
        m_RePlayButton.gameObject.SetActive(true);
    }

    //hiding results buttons
    public void HideResultButtons()
    {
        m_ExitButton.gameObject.SetActive(false);
        m_RePlayButton.gameObject.SetActive(false);
    }
    #endregion

    //-----------------------------------------------------------------

}
