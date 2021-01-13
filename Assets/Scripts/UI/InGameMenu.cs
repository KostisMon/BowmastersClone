using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The Menu containing the In game panel component
public class InGameMenu : Menu
{
    #region Variables

    private InGamePanelComponent m_InGamePanelComponent;
    private List<MenuComponent> m_InGameMenuPanelComponentList = new List<MenuComponent>();

    #endregion

    //-----------------------------------------------------------------

    #region Public Methods
    //Creating a new in game component and adding it to the list

    public override void Awake()
    {
        base.Awake();
        m_InGamePanelComponent = new InGamePanelComponent(this);

        m_InGameMenuPanelComponentList.Add(m_InGamePanelComponent);

    }

    //used for initialization
    public void Init()
    {
        GameManager.Instance.OnStateChange += CheckState;
    }

    public override void Update()
    {
        base.Update();
        //updates based on game state
        StateUpdate();
    }

    //used for health reset
    public void MaxHealth()
    {
        m_InGamePanelComponent.MaxHealth();
    }

    public override void Start()
    {
        base.Start();
        ShowInGamePanelComponent(true);
    }

    //hiding all panels
    public void HideAllPanels()
    {
        for (int i = 0; i < m_InGameMenuPanelComponentList.Count; i++)
        {
            m_InGameMenuPanelComponentList[i].ShowComponent(false);
        }
    }
    
    //used to update the Distance Indicator UI
    public void UpdateDistanceUiPos()
    {
        m_InGamePanelComponent.UpdateDistanceUiPos();
    }

    //used to initialise the Distance Indicator UI
    public void InitialiseDistanceUi()
    {
        m_InGamePanelComponent.InitialiseDistanceUi();
    }

    //used to show/hide Distance Indicator UI
    public void ShowCharDistanceUi(bool show)
    {
        m_InGamePanelComponent.ShowCharDistanceUi(show);
    }

    //Showing/hiding in game panel
    public void ShowInGamePanelComponent(bool show)
    {
        HideAllPanels();
        m_InGamePanelComponent.ShowComponent(show);
    }

    //showing/hiding trajectory info ui
    public void ShowTrajectoryInfo(bool show)
    {
        m_InGamePanelComponent.ShowTrajectoryInfo(show);
    }

    //setting power ui text
    public void SetPowerText(float power)
    {
        m_InGamePanelComponent.SetPowerText(power);
    }

    //setting angle ui text
    public void SetAngleText(float angle)
    {
        m_InGamePanelComponent.SetAngleText(angle);
    }

    //updates health depending on player type
    public void UpdateCharHealth(Player.PlayerType type, float curHealth, float maxHealth)
    {
        m_InGamePanelComponent.UpdateCharHeath(type, curHealth, maxHealth);
    }

    #endregion

    //-----------------------------------------------------------------

    #region Private Methods
    //used for state related updates
    private void StateUpdate()
    {

        GameState curState = GameManager.Instance.GetState();
        switch (curState)
        {
            case GameState.MainMenu:
                break;
            case GameState.PlayerAims:
                //setting power and angle text
                SetAngleText(TrajectoryManager.Instance.Angle);
                SetPowerText(TrajectoryManager.Instance.Power);
                if (TrajectoryManager.Instance.Power > 1f)
                {
                    //if it's above certain amount we show the ui
                    ShowTrajectoryInfo(true);
                }
                else
                {
                    ShowTrajectoryInfo(false);
                }
                //updating the distance 
                UpdatePlayerDistance(GameManager.Instance.CalculateCharDistance());
                // updating the position of the distance indicator ui
                UpdateDistanceUiPos();
                break;
            case GameState.PlayerShoots: //when player shoots
                //updating distance
                UpdatePlayerDistance(GameManager.Instance.CalculateCharDistance());
                //showing/hiding distance indicator if char visible
                ShowCharDistanceUi(!CameraManager.Instance.CheckIfCharVisible(Player.PlayerType.Enemy));
                //updating the position of the distance indicator ui
                UpdateDistanceUiPos();
                break;
            case GameState.EnemyAims://when enemy aims
                //updating distance
                UpdatePlayerDistance(GameManager.Instance.CalculateCharDistance());
                //updating distance indicator ui position
                UpdateDistanceUiPos();

                break;
            case GameState.EnemyShoots: //wheen enemy shoots
                //updating distance
                UpdatePlayerDistance(GameManager.Instance.CalculateCharDistance());
                //showing/hiding distance indicator ui
                ShowCharDistanceUi(!CameraManager.Instance.CheckIfCharVisible(Player.PlayerType.Player));
                //updating distance indicator ui position
                UpdateDistanceUiPos();

                break;
        }
    }

    //used for updating distance text
    private void UpdatePlayerDistance(float distance)
    {
        m_InGamePanelComponent.UpdatePlayerDistance(distance);
    }

    //one time check and method call depending on current Game State
    private void CheckState()
    {
        GameState curState = GameManager.Instance.GetState();
        switch (curState)
        {
            case GameState.PlayerAims:
                ShowCharDistanceUi(true);
                InitialiseDistanceUi();
                break;
            case GameState.PlayerShoots:
                ShowTrajectoryInfo(false);

                break;
            case GameState.EnemyAims:
                ShowCharDistanceUi(true);
                InitialiseDistanceUi();
                ShowTrajectoryInfo(false);

                break;
            case GameState.EnemyShoots:
                ShowTrajectoryInfo(false);

                break;
            case GameState.EndGame:
                ShowTrajectoryInfo(false);

                break;
            default:
                break;
        }
    }

    #endregion
    //-----------------------------------------------------------------

}
