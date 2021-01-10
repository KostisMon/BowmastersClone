using Cinemachine;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum GameState { MainMenu, PlayerAims, PlayerShoots, EnemyAims, EnemyShoots, EndGame }
public delegate void OnStateChangeHandler();


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

    public static bool IsAppPaused;
    public event OnStateChangeHandler OnStateChange;
    public Action OnAppPaused = delegate { };
    public Action OnAppUnpaused = delegate { };

    [Header("Camera System")]
    public CinemachineBrain VcamBrain;
    public CinemachineVirtualCamera PlayerVcam;
    public CinemachineVirtualCamera EnemyVcam;
    public CinemachineVirtualCamera ProjectileVcam;

    [Header("Shooting System")]
    public GameObject ProjectilePrefab;
    public GameObject CurrentProjectile;
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
    [SerializeField] [Range(0.01f, 0.05f)] public float TrajectoryPointMinScale;
    [SerializeField] [Range(0.05f, 1f)] public float TrajectoryPointMaxScale;

    [Header("UI System")]
    public TextMeshProUGUI PowerText;
    public TextMeshProUGUI AngleText;
    public GameObject TrajectoryCanvasObj;
    public GameObject MainMenu;
    public Image PlayerHealthImage;
    public Image EnemyHealthImage;
    #endregion

    //-----------------------------------------------------------------

    #region Private Methods
    private void Awake()
    {
        s_Instance = this;
        SetState(GameState.MainMenu);

    }

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        Application.targetFrameRate = 30;
        UiManager.Instance.Init();
        CameraManager.Instance.Init();
        InputManager.Instance.Init();
        TrajectoryManager.Instance.Init();

    }

    private void Update()
    {
        InputManager.Instance.Update();
        CameraManager.Instance.Update();
        UiManager.Instance.Update();
    }

    private void FixedUpdate()
    {
        TrajectoryManager.Instance.FixedUpdate();
    }

    private void OnApplicationPause(bool pause)
    {
        IsAppPaused = pause;
        if (pause)
        {
            OnAppPaused();
        }
        else
        {
            OnAppUnpaused();
        }
    }
    #endregion

    //-----------------------------------------------------------------

    #region Public Methods
    public void SetState(GameState state)
    {
        m_CurGameState = state;
        if (OnStateChange != null)
        {
            OnStateChange();
        }
    }

    public GameState GetState()
    {
        return m_CurGameState;
    }

    public void ShootProjectile(Vector2 force)
    {
        Quaternion rotation = Quaternion.AngleAxis(TrajectoryManager.Instance.Angle, Vector3.forward);
        GameObject projectile = Instantiate(ProjectilePrefab, TrajectoryStartPos.transform.position, rotation);
        CurrentProjectile = projectile;
        CurrentProjectile.GetComponent<Projectile>().Shoot(force);
    }

    public void PlayerWon()
    {
        throw new NotImplementedException();
    }

    public void EnemyWon()
    {
        throw new NotImplementedException();
    }

    #endregion
    //-----------------------------------------------------------------

}
