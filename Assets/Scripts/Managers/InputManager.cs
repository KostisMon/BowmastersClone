using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    //-----------------------------------------------------------------

    #region Variables
    private Vector2 m_StartPoint, m_EndPoint;

    private bool m_Aiming;
    public bool Aiming {
        get { return m_Aiming; }
    }
    
    #endregion

    //-----------------------------------------------------------------

    #region Public Methods

    public void Init()
    {
        m_Aiming = false;
    }

    public void Update()
    {
        if (GameManager.Instance.GetState() == GameState.PlayerAims)
        {
            if (m_Aiming)
            {
                m_EndPoint =  Input.mousePosition;
                
                TrajectoryManager.Instance.CalculateTrajectory(m_StartPoint, m_EndPoint);
                GameManager.Instance.Player.ArmsFollow(m_EndPoint);
                CameraManager.Instance.SetCameraSize(TrajectoryManager.Instance.Distance);



            }
            if (!m_Aiming && Input.GetMouseButtonDown(0))
            {
                m_StartPoint =  Input.mousePosition;

                m_Aiming = true;
                
            }
            if (Input.GetMouseButtonUp(0))
            {
                m_Aiming = false;
                if (Mathf.Abs(TrajectoryManager.Instance.ForceVector.x )>1f)
                {
                    GameManager.Instance.ShootProjectile(TrajectoryManager.Instance.ForceVector);
                    GameManager.Instance.SetState(GameState.PlayerShoots);

                }

                TrajectoryManager.Instance.ShowTrajectory(false);
                UiManager.Instance.ShowPowerAndAngleUi(false);
                
                
            }
        }
    }

   
    #endregion

    //-----------------------------------------------------------------

    #region Private Methods

    

    #endregion

    //-----------------------------------------------------------------


}
