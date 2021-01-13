using UnityEngine;
using UnityEngine.UI;

//Menu Component used for the start menu of the game
public class StartMenuPanelComponent : MenuComponent
{

    //-----------------------------------------------------------------

    #region Variables
    private Button m_PlayButton, m_ExitButton;

    #endregion

    //-----------------------------------------------------------------

    #region Public Methods
    // Constructor, searching and populating variables
    // Plus setting methods for buttons
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
            if (trans[i].name == "ExitButton")
            {
                m_ExitButton = trans[i].GetComponent<Button>();
                SetMethod(m_ExitButton, ExitGame);
            }
        }
    }



    #endregion
    //-----------------------------------------------------------------

    #region Private Methods
    //called from Exit button
    private void ExitGame()
    {
        Application.Quit();
    } 

    //Called from PlayButton
    private void PlayGame()
    {
        //Reseting power and angle
        TrajectoryManager.Instance.ResetPowerAndAngle();
        //Reseting Player and Enemy Health
        GameManager.Instance.Player.MaxHealth();
        GameManager.Instance.Enemy.MaxHealth();
        //Resets the UI health indicator
        UiManager.Instance.InGameMenu.MaxHealth();
        //Setting starting states
        UiManager.Instance.SetState(UiManager.MenuState.InGameMenu);
        GameManager.Instance.SetState(GameState.PlayerAims);

    }
    #endregion

    //-----------------------------------------------------------------

}
