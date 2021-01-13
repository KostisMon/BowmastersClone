using Cinemachine;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    //-----------------------------------------------------------------

    #region Variables
    //made 3 camera states to use combined with Cinemachine 2d
    public enum VirtualCamTypes { Player, Enemy, Projectile}
    private VirtualCamTypes m_CurActiveCamera;
    private float m_CamInitialSize;

    private CinemachineVirtualCamera m_ActiveVirtualCamera;
    private CinemachineVirtualCamera m_PrevActiveVirtualCamera;
    #endregion

    //-----------------------------------------------------------------

    #region Public Methods
    //Used for initialization in GameManager
    public void Init()
    {
        //Saving the initial camera size
        m_CamInitialSize = Camera.main.orthographicSize;
        //setting up the starting Vcam 
        m_ActiveVirtualCamera = GameManager.Instance.PlayerVcam;
        //Adding a method to the GameManager state change event
        GameManager.Instance.OnStateChange += CheckState;
    }
    
    //used for the Distance UI
    public bool CheckIfCharVisible(Player.PlayerType type)
    {
        //temp position
        Vector3 viewPos = Vector3.zero;
        switch (type)
        {
            //if type is Player we get the Viewport Point of the Player game object
            case Player.PlayerType.Player:
                viewPos = Camera.main.WorldToViewportPoint(GameManager.Instance.Player.transform.position);

                break;
            // else we get the Viewport Point from the Enemy
            case Player.PlayerType.Enemy:
                viewPos = Camera.main.WorldToViewportPoint(GameManager.Instance.Enemy.transform.position);

                break;
        }
        //if object in view return true else return false
        if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
        {
            return true;
        }
        else
        {
            return false;
        }


    }

    //Used to change bewteen the 3 virtual Cameras
    public void SetActiveVirtualCamera(GameState state, CinemachineVirtualCamera vCam)
    {
        //saving the previous vcam
        m_PrevActiveVirtualCamera = m_ActiveVirtualCamera;
        //and assigning the new one
        m_ActiveVirtualCamera = vCam;
        
        //disabling all of them to begin with
        GameManager.Instance.EnemyVcam.gameObject.SetActive(false);
        GameManager.Instance.ProjectileVcam.gameObject.SetActive(false);
        GameManager.Instance.PlayerVcam.gameObject.SetActive(false);
        
        //checking we are in Shoot State (cause both ones use the Projectile vcam)
        if (state == GameState.PlayerShoots || state == GameState.EnemyShoots)
        {
            //assingin the current orthographic size to the active vcam
            m_ActiveVirtualCamera.m_Lens.OrthographicSize = m_PrevActiveVirtualCamera.m_Lens.OrthographicSize;
            //and assigning the current projeticle to the Follow attribute of the Vcam
            SetCinemachineFollowTransform(GameManager.Instance.GetCurrentProjectile().transform);
        }
        //in the end we enable the current Vcam
        m_ActiveVirtualCamera.gameObject.SetActive(true);
    }

    //used for assigning the active Vcam a Transform to follow
    public void SetCinemachineFollowTransform(Transform followTransform)
    {
        m_ActiveVirtualCamera.Follow = followTransform;
    }

    //used for zooming in/out whem Aiming
    public void SetCameraSize(float distance)
    {
        //calculating the orthographic size, adding the to initial size the distance between touches
        float camOrthoSize = distance + m_CamInitialSize;
        //clamping the value between initial size and max size (used 8 after testing)
        camOrthoSize = Mathf.Clamp(camOrthoSize, m_CamInitialSize, 8f);
        //assigning the clamped float to the active vcam ortho size
        m_ActiveVirtualCamera.m_Lens.OrthographicSize = camOrthoSize;
    }


    #endregion

    //-----------------------------------------------------------------

    #region Private Methods
    //this gets called every time GameState changes
    private void CheckState()
    {
        GameState curState = GameManager.Instance.GetState();
        switch (curState)
        {
            //I assing the proper current active vCam
            //When needed refresh the Vcams
            case GameState.PlayerAims:
                RefreshCameras();
                SetActiveVirtualCamera(curState, GameManager.Instance.PlayerVcam);
                break;
            case GameState.PlayerShoots:
                SetActiveVirtualCamera(curState, GameManager.Instance.ProjectileVcam);
                break;
            case GameState.EnemyAims:
                RefreshCameras();
                SetActiveVirtualCamera(curState, GameManager.Instance.EnemyVcam);
                break;
            case GameState.EnemyShoots:
                SetActiveVirtualCamera(curState, GameManager.Instance.ProjectileVcam);
                break;
            case GameState.EndGame:
                SetActiveVirtualCamera(curState, GameManager.Instance.PlayerVcam);
                break;
            default:
                break;
        }
    }

    //Refreshing the orthographic size to the initial one
    private void RefreshCameras()
    {
        GameManager.Instance.PlayerVcam.m_Lens.OrthographicSize = m_CamInitialSize;
        GameManager.Instance.EnemyVcam.m_Lens.OrthographicSize = m_CamInitialSize;
    }

    #endregion

    //-----------------------------------------------------------------

}
