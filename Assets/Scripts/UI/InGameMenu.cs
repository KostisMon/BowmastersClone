using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : Menu
{
    #region Variables

    private InGamePanelComponent m_InGamePanelComponent;
    private List<MenuComponent> m_InGameMenuPanelComponentList = new List<MenuComponent>();

    #endregion

    //-----------------------------------------------------------------

    #region Public Methods
    public override void Awake()
    {
        base.Awake();
        m_InGamePanelComponent = new InGamePanelComponent(this);

        m_InGameMenuPanelComponentList.Add(m_InGamePanelComponent);

    }

    public void Init()
    {
        GameManager.Instance.OnStateChange += CheckState;
    }

    public override void Update()
    {
        base.Update();

        GameState curState = GameManager.Instance.GetState();
        switch (curState)
        {
            case GameState.MainMenu:
                break;
            case GameState.PlayerAims:
                SetAngleText(TrajectoryManager.Instance.Angle);
                SetPowerText(TrajectoryManager.Instance.Power);
                if (TrajectoryManager.Instance.Power > 1f)
                {
                    ShowTrajectoryInfo(true);
                   
                }
                UpdatePlayerDistance(GameManager.Instance.CalculateCharDistance());
                UpdateDistanceUiPos();
                break;
            case GameState.PlayerShoots:
                UpdatePlayerDistance(GameManager.Instance.CalculateCharDistance());
                ShowCharDistanceUi(!CameraManager.Instance.CheckIfCharVisible(Player.PlayerType.Enemy));
                UpdateDistanceUiPos();
                break;
            case GameState.EnemyAims:
                UpdatePlayerDistance(GameManager.Instance.CalculateCharDistance());
                UpdateDistanceUiPos();

                break;
            case GameState.EnemyShoots:
                UpdatePlayerDistance(GameManager.Instance.CalculateCharDistance());
                ShowCharDistanceUi(!CameraManager.Instance.CheckIfCharVisible(Player.PlayerType.Player));
                UpdateDistanceUiPos();

                break;
            case GameState.EndGame:
                break;
            default:
                break;
        }

    }

    public override void Start()
    {
        base.Start();
        ShowInGamePanelComponent(true);
    }

    public void HideAllPanels()
    {
        for (int i = 0; i < m_InGameMenuPanelComponentList.Count; i++)
        {
            m_InGameMenuPanelComponentList[i].ShowComponent(false);
        }
    }

    public void UpdateDistanceUiPos()
    {
        m_InGamePanelComponent.UpdateDistanceUiPos();
    }

    public void InitialiseDistanceUi()
    {
        m_InGamePanelComponent.InitialiseDistanceUi();
    }

    public void ShowCharDistanceUi(bool show)
    {
        m_InGamePanelComponent.ShowCharDistanceUi(show);
    }

    public void ShowInGamePanelComponent(bool show)
    {
        HideAllPanels();
        m_InGamePanelComponent.ShowComponent(show);
    }

    public void ShowTrajectoryInfo(bool show)
    {
        m_InGamePanelComponent.ShowTrajectoryInfo(show);
    }

    public void SetPowerText(float power)
    {
        m_InGamePanelComponent.SetPowerText(power);
    }

    public void SetAngleText(float angle)
    {
        m_InGamePanelComponent.SetAngleText(angle);
    }

    public void UpdateEnemyHealth(float curHealth, float maxHealth)
    {
        m_InGamePanelComponent.UpdateEnemyHeath(curHealth, maxHealth);
    }

    public void UpdatePlayerHealth(float curHealth, float maxHealth)
    {
        m_InGamePanelComponent.UpdatePlayerHeath(curHealth, maxHealth);
    }

    #endregion

    //-----------------------------------------------------------------

    #region Private Methods
    private void UpdatePlayerDistance(float distance)
    {
        m_InGamePanelComponent.UpdatePlayerDistance(distance);
    }

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
                
                break;
            case GameState.EnemyAims:
                ShowCharDistanceUi(true);
                InitialiseDistanceUi();
                break;
            case GameState.EnemyShoots:
               
                break;
            case GameState.EndGame:
                break;
            default:
                break;
        }
    }

    #endregion
    //-----------------------------------------------------------------

}
