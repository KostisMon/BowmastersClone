using UnityEngine;
using System;

public enum GameState { MainMenu, PlayerPlays, EnemyPlays }
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
    public GameState GameState { get; private set; }

    public static bool IsAppPaused;
    public event OnStateChangeHandler OnStateChange;
    public Action OnAppPaused = delegate { };
    public Action OnAppUnpaused = delegate { };

    public GameObject Projectile;

    public float ForceMultiplier;

    [Header("Trajectory System")]
    public GameObject TrajectoryPoint;
    public GameObject TrajectoryStartPos;
    public GameObject TrajectoryParent;
    public int TrajectoryPointCount;
    public float TrajectoryPointSpacing;
    [SerializeField] [Range(0.01f, 0.5f)] public float TrajectoryPointMinScale;
    [SerializeField] [Range(0.5f, 1.5f)] public float TrajectoryPointMaxScale;

    #endregion

    //-----------------------------------------------------------------

    #region Private Methods
    private void Awake()
    {
        s_Instance = this;
        SetState(GameState.PlayerPlays);
    }

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        Application.targetFrameRate = 30;

        CameraManager.Instance.Init();
        InputManager.Instance.Init();
        TrajectoryManager.Instance.Init();
    }

    private void Update()
    {
        InputManager.Instance.Update();
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
        this.GameState = state;
        if (OnStateChange != null)
        {
            OnStateChange();
        }
    }

    public GameState GetState()
    {
        return this.GameState;
    }

    public void ShootProjectile(Vector2 force)
    {
        GameObject projectile = Instantiate(Projectile, TrajectoryStartPos.transform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().Push(force);
    }
    #endregion
    //-----------------------------------------------------------------

}
