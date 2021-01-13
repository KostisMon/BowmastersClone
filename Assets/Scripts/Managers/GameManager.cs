using Cinemachine;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum GameState { MainMenu, PlayerAims, PlayerShoots, EnemyAims, EnemyShoots, EndGame }
public delegate void OnStateChangeHandler();

//Core Manager of the game. Here the singleton is applied from within the classs
public class GameManager : MonoBehaviour
{
    //-----------------------------------------------------------------

    #region Singleton

    private static GameManager s_Instance;
    public static GameManager Instance
    {
        get { return s_Instance; }
    }

    #endregion

    //-----------------------------------------------------------------

    #region Variables
    private GameState m_CurGameState;

    public event OnStateChangeHandler OnStateChange;

    [Header("Camera System")]
    public CinemachineBrain VcamBrain;
    public CinemachineVirtualCamera PlayerVcam;
    public CinemachineVirtualCamera EnemyVcam;
    public CinemachineVirtualCamera ProjectileVcam;

    [Header("Shooting System")]
    public GameObject ProjectilePrefab;
    private GameObject m_CurrentProjectile;
    public GameObject ProjectileParent;
    public float PowerMultiplier;

    [Header("Player System")]
    public Player Player;
    public Player Enemy;

    [Header("Trajectory System")]
    public GameObject TrajectoryPoint;
    public GameObject TrajectoryStartPos;
    public GameObject TrajectoryParent;
    public int TrajectoryPointCount;
    public float TrajectoryPointSpacing;
    [Range(0.01f, 0.05f)] public float TrajectoryPointMinScale;
    [Range(0.05f, 1f)] public float TrajectoryPointMaxScale;

    [Header("Audio Clips")]
    public AudioClip ThrowSound;
    public AudioClip HitSound;
    public AudioClip WinSound;
    public AudioClip LoseSound;

    #endregion

    //-----------------------------------------------------------------

    #region Private Methods
    //Setting the start State and calls Awake for the UI to be ready
    private void Awake()
    {
        s_Instance = this;
        SetState(GameState.MainMenu);
        UiManager.Instance.Awake();
    }

    //Initialises all the Managers
    private void Start()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        Application.targetFrameRate = 30;
        UiManager.Instance.Init();
        AudioManager.Instance.Init();
        CameraManager.Instance.Init();
        InputManager.Instance.Init();
        TrajectoryManager.Instance.Init();

    }
    //Calling the Updates
    private void Update()
    {
        InputManager.Instance.Update();
        TrajectoryManager.Instance.Update();
    }

    #endregion

    //-----------------------------------------------------------------

    #region Public Methods
    //used for swapping states
    public void SetState(GameState state)
    {
        m_CurGameState = state;
        if (OnStateChange != null)
        {
            OnStateChange();
        }
    }

    //Getting the current state
    public GameState GetState()
    {
        return m_CurGameState;
    }

    //Just a float scale method (used for making force look from 0-100 in the UI)
    public float FloatScale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {
        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;
        return (NewValue);
    }

    //Used to Shoot projectile from the player
    public void PlayerShootsProjectile(Vector2 force)
    {
        //creating a rotation 
        Quaternion rotation = Quaternion.AngleAxis(TrajectoryManager.Instance.Angle, Vector3.forward);
        //Instantiating
        GameObject projectile = Instantiate(ProjectilePrefab, Player.ProjectileStartPos.transform.position, rotation);
        //Parenting (for tidyness)
        ParentProjectile(projectile);
        //Setting this as the current projectile
        SetCurrentProjectile(projectile);
        //And shoots it from the Projectile script
        projectile.GetComponent<Projectile>().Shoot(force);
    }

    //used for parenting projectiles
    public void ParentProjectile(GameObject projectile)
    {
        projectile.transform.parent = ProjectileParent.transform; 
    }

    //used to clear projectiles from parent
    public void ClearProjectiles()
    {
        foreach (Transform child in ProjectileParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

    }

    //used for setting the current projectile
    public void SetCurrentProjectile(GameObject projectile)
    {
        m_CurrentProjectile = projectile;
    }
    
    //used for getting the current projectile
    public GameObject GetCurrentProjectile()
    {
        return m_CurrentProjectile;
    }

    //used to get the Char distance from the projectile
    public float CalculateCharDistance()
    {
        float charDistance =0f;
        switch (m_CurGameState)
        {
            //when  aiming we get the distance between characters
            case GameState.PlayerAims:
                charDistance = Vector2.Distance(Player.transform.position, Enemy.transform.position);
                break;
            //when shooting we get the distance to the projectile
            case GameState.PlayerShoots:
                charDistance = Vector2.Distance(m_CurrentProjectile.transform.position, Enemy.transform.position);
                break;
            case GameState.EnemyAims:
                charDistance = Vector2.Distance(Player.transform.position, Enemy.transform.position);
                break;
            case GameState.EnemyShoots:
                charDistance = Vector2.Distance(m_CurrentProjectile.transform.position, Player.transform.position);
                break;
        }
        return charDistance;

    }

    //used to check what state is next after projectile hits
    public void CheckStateAfterHit()
    {
        if (m_CurGameState == GameState.EnemyShoots)
        {
            SetState(GameState.PlayerAims);
        }
        else if (m_CurGameState == GameState.PlayerShoots)
        {
            SetState(GameState.EnemyAims);
        }
    }

    //called when Player wins
    public void PlayerWon()
    {
        //setting game state
        SetState(GameState.EndGame);
        //setting UI state
        UiManager.Instance.SetState(UiManager.MenuState.ResultMenu);
        //calls the Result menu to start Win sequence
        UiManager.Instance.ResultMenu.Won();
        //Plays win sound
        AudioManager.Instance.PlayWinSound();
    }

    //called when Player loses
    public void EnemyWon()
    {
        //setting game state
        SetState(GameState.EndGame);
        //setting UI state
        UiManager.Instance.SetState(UiManager.MenuState.ResultMenu);
        //calls the Result menu to start Win sequence
        UiManager.Instance.ResultMenu.Lost();
        //Plays lose sound
        AudioManager.Instance.PlayLoseSound();
    }

    #endregion
    //-----------------------------------------------------------------

}
