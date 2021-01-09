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
        if (GameManager.Instance.GameState == GameState.PlayerPlays)
        {
            if (m_Aiming)
            {
                m_EndPoint =  Input.mousePosition;
                
                CameraManager.Instance.SetCameraSize(TrajectoryManager.Instance.Distance);
                TrajectoryManager.Instance.CalculateTrajectory(m_StartPoint, m_EndPoint);

                

                Vector3 mousePosition = Input.mousePosition;
                mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                Vector2 directionLeft = new Vector2(
                mousePosition.x - GameManager.Instance.LeftHand.transform.position.x,
                mousePosition.y - GameManager.Instance.LeftHand.transform.position.y
                );
                Vector2 directionRight = new Vector2(
                mousePosition.x - GameManager.Instance.RightHand.transform.position.x,
                mousePosition.y - GameManager.Instance.RightHand.transform.position.y
                );
                GameManager.Instance.RightHand.transform.up = directionLeft;

                GameManager.Instance.LeftHand.transform.up = directionLeft;


            }
            if (!m_Aiming && Input.GetMouseButtonDown(0))
            {
                //m_StartPoint = Camera.main.ScreenToWorldPoint( Input.mousePosition);
                m_StartPoint =  Input.mousePosition;

                m_Aiming = true;
                
            }
            if (Input.GetMouseButtonUp(0))
            {

                m_Aiming = false;

                GameManager.Instance.ShootProjectile(TrajectoryManager.Instance.ForceVector);

                TrajectoryManager.Instance.ShowTrajectory(false);
                UiManager.Instance.ShowPowerAndAngleUi(false);
                
                //GameManager.Instance.SetState(GameState.EnemyPlays);
            }
        }
    }

   
    #endregion

    //-----------------------------------------------------------------

    #region Private Methods

    

    #endregion

    //-----------------------------------------------------------------


}
