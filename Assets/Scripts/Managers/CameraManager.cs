using Cinemachine;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    //-----------------------------------------------------------------

    #region Variables
    public enum VirtualCamTypes { Player, Enemy, Projectile}
    private VirtualCamTypes m_CurActiveCamera;
    private float m_CamInitialSize;
    private Camera m_MainCamera;

    private CinemachineVirtualCamera m_ActiveVirtualCamera;
    private CinemachineVirtualCamera m_PrevActiveVirtualCamera;
    #endregion

    //-----------------------------------------------------------------

    #region Public Methods
    public void Init()
    {
        m_CamInitialSize = Camera.main.orthographicSize;
        m_MainCamera = Camera.main;
        m_ActiveVirtualCamera = GameManager.Instance.PlayerVcam;
        GameManager.Instance.OnStateChange += CheckState;
    }


    public void Update()
    {
       
    }

    public bool CheckIfCharVisible(Player.PlayerType type)
    {
        Vector3 viewPos = Vector3.zero;
        switch (type)
        {
            case Player.PlayerType.Player:
                viewPos = Camera.main.WorldToViewportPoint(GameManager.Instance.Player.transform.position);

                break;
            case Player.PlayerType.Enemy:
                viewPos = Camera.main.WorldToViewportPoint(GameManager.Instance.Enemy.transform.position);

                break;
        }
        if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
        {
            return true;
        }
        else
        {
            return false;
        }


    }
    public void SetActiveVirtualCamera(GameState state, CinemachineVirtualCamera vCam)
    {
        m_PrevActiveVirtualCamera = m_ActiveVirtualCamera;
        m_ActiveVirtualCamera = vCam;

        GameManager.Instance.EnemyVcam.gameObject.SetActive(false);
        GameManager.Instance.ProjectileVcam.gameObject.SetActive(false);
        GameManager.Instance.PlayerVcam.gameObject.SetActive(false);
        
        if (state == GameState.PlayerShoots || state == GameState.EnemyShoots)
        {
            m_ActiveVirtualCamera.m_Lens.OrthographicSize = m_PrevActiveVirtualCamera.m_Lens.OrthographicSize;
            SetCinemachineFollowTransform(GameManager.Instance.CurrentProjectile.transform);
        }

        vCam.gameObject.SetActive(true);
    }

    public void SetCinemachineFollowTransform(Transform followTransform)
    {
        m_ActiveVirtualCamera.Follow = followTransform;
    }

    public void SetCameraSize(float distance)
    {
        m_ActiveVirtualCamera.m_Lens.OrthographicSize = distance + m_CamInitialSize;
        m_ActiveVirtualCamera.m_Lens.OrthographicSize = Mathf.Clamp(m_ActiveVirtualCamera.m_Lens.OrthographicSize, 3f, 8f);
        GameManager.Instance.ProjectileVcam.m_Lens.OrthographicSize = distance + m_CamInitialSize;
        GameManager.Instance.ProjectileVcam.m_Lens.OrthographicSize = Mathf.Clamp(GameManager.Instance.ProjectileVcam.m_Lens.OrthographicSize, 3f, 8f);

    }


    #endregion

    //-----------------------------------------------------------------

    #region Private Methods
    private void CheckState()
    {
        GameState curState = GameManager.Instance.GetState();
        switch (curState)
        {
            case GameState.MainMenu:
                break;
            case GameState.PlayerAims:
                RefreshCameras();
                SetActiveVirtualCamera(curState, GameManager.Instance.PlayerVcam);
                break;
            case GameState.PlayerShoots:
                SetActiveVirtualCamera(curState, GameManager.Instance.ProjectileVcam);
                break;
            case GameState.EnemyAims:
                SetActiveVirtualCamera(curState, GameManager.Instance.EnemyVcam);
                break;
            case GameState.EnemyShoots:
                SetActiveVirtualCamera(curState, GameManager.Instance.ProjectileVcam);
                break;
            case GameState.EndGame:
                break;
            default:
                break;
        }
    }

    private void RefreshCameras()
    {
        GameManager.Instance.PlayerVcam.m_Lens.OrthographicSize = m_CamInitialSize;
        GameManager.Instance.EnemyVcam.m_Lens.OrthographicSize = m_CamInitialSize;
    }

    #endregion

    //-----------------------------------------------------------------

}
