using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    //-----------------------------------------------------------------

    #region Variables
    private bool m_Aiming;
    private float m_Distance;
    private Vector2 m_Force;
    private Vector2 m_Direction;
    private Vector2 m_StartPoint, m_EndPoint;
    
    public bool Aiming {
        get { return m_Aiming; }
    }
    
    public Vector2 Force
    {
        get { return m_Force; }
    }
    public Vector2 Direction
    {
        get { return m_Direction; }
    }

    public void Init()
    {
        m_Aiming = false;
    }
    #endregion

    //-----------------------------------------------------------------

    #region Public Methods
    public void Update()
    {
        if (GameManager.Instance.GameState == GameState.PlayerPlays)
        {
            if (m_Aiming)
            {
                m_EndPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                m_Distance = Vector2.Distance(m_StartPoint, m_EndPoint);
                m_Direction = (m_StartPoint - m_EndPoint).normalized;
                m_Force = m_Direction * m_Distance * GameManager.Instance.ForceMultiplier;

                CameraManager.Instance.SetCameraSize(m_Distance);


                if (!GameManager.Instance.TrajectoryParent.activeSelf)
                {
                    if (Mathf.Abs(m_Force.x) > 1f || Mathf.Abs(m_Force.y) > 1f  )
                    {
                        TrajectoryManager.Instance.ShowTrajectory(true);
                    }
                }
                


            }
            if (!m_Aiming && Input.GetMouseButtonDown(0))
            {
                //GameManager.Instance.Projectile.de
                m_StartPoint = Camera.main.ScreenToWorldPoint( Input.mousePosition);
                m_Aiming = true;
                
            }
            if (Input.GetMouseButtonUp(0))
            {

                m_Aiming = false;
                GameManager.Instance.ShootProjectile(m_Force);
                TrajectoryManager.Instance.ShowTrajectory(false);
            }
        }
    }
    #endregion

    //-----------------------------------------------------------------

    #region Private Methods
    private void Shoot()
    {
        GameObject arrow = GameObject.Instantiate(GameManager.Instance.Projectile);
        Rigidbody2D arrowRigidBody = arrow.GetComponent<Rigidbody2D>();

        arrowRigidBody.velocity = m_Direction * m_Force;
        arrowRigidBody.simulated = true;

    }
    #endregion

    //-----------------------------------------------------------------
}
