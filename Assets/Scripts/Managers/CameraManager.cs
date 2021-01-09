using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : Singleton<CameraManager>
{
    //-----------------------------------------------------------------

    #region Variables
    private float m_CamInitialSize;
    private CinemachineVirtualCamera m_VirtualCamera;
    private Camera m_MainCamera;
    #endregion

    //-----------------------------------------------------------------

    #region Public Methods
    public void Init()
    {
        m_CamInitialSize = Camera.main.orthographicSize;
        m_MainCamera = Camera.main;
        m_VirtualCamera = GameManager.Instance.Cinemachine;//GetVirtualCamera();
        
    }
    
    public void Update()
    {
       
    }

    public void SetCinemachineFollowTransform(Transform followTransform)
    {

       // m_VirtualCamera.Follow = followTransform;
    }

    public void SetCameraSize(float distance)
    {
        m_VirtualCamera.m_Lens.OrthographicSize = distance + m_CamInitialSize;
        m_VirtualCamera.m_Lens.OrthographicSize = Mathf.Clamp(m_VirtualCamera.m_Lens.OrthographicSize, 3f, 8f);
    }
    #endregion

    //-----------------------------------------------------------------
}
