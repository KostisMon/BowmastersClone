using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    //-----------------------------------------------------------------

    #region Variables
    private float m_CamInitialSize;
    private Camera m_MainCamera;
    #endregion

    //-----------------------------------------------------------------

    #region Public Methods
    public void Init()
    {
        m_CamInitialSize = Camera.main.orthographicSize;
        m_MainCamera = Camera.main;
    }
    
    public void Update()
    {

    }


    public void SetCameraSize(float distance)
    {
        m_MainCamera.orthographicSize = distance + m_CamInitialSize;
        m_MainCamera.orthographicSize = Mathf.Clamp(m_MainCamera.orthographicSize, 3f, 8f);
    }
    #endregion

    //-----------------------------------------------------------------
}
