using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used for controlling the interaction from the Player (When it is his/her turn)
public class InputManager : Singleton<InputManager>
{
    //-----------------------------------------------------------------

    #region Variables
    //used for touch start/end position
    private Vector2 m_StartPoint, m_CurrentPoint;

    //used for checking the aim state
    private bool m_Aiming;
    public bool Aiming {
        get { return m_Aiming; }
    }
    
    #endregion

    //-----------------------------------------------------------------

    #region Public Methods
    // Should start always with Aiming disabled untill touch happen
    public void Init()
    {
        m_Aiming = false;
    }

    public void Update()
    {
        //Checking Input
        CheckInput();
    }

   
    #endregion

    //-----------------------------------------------------------------

    #region Private Methods
    //All major actions concerning touch input happens here
    private void CheckInput()
    {
        // for all to work I need to be in the correct state
        if (GameManager.Instance.GetState() == GameState.PlayerAims)
        {
            //if player touches the screen
            if (m_Aiming)
            {
                // I save the current postion of the touch
                m_CurrentPoint = Input.mousePosition;
                // Using Trajectory Manage to calculate the aim trajectory based on start and current touch position
                TrajectoryManager.Instance.CalculateTrajectory(m_StartPoint, m_CurrentPoint);
                // Instructs the character's Arms to folow the current touch position
                GameManager.Instance.Player.ArmsFollow(m_CurrentPoint);
                // Applying some Zoom out based on the distance between touch positions
                CameraManager.Instance.SetCameraSize(TrajectoryManager.Instance.Distance);
            }
            // I need to be the first touch
            if (!m_Aiming && Input.GetMouseButtonDown(0))
            {
                //saves the start point of the touch
                m_StartPoint = Input.mousePosition;
                //setting the aim true so that calculations could start
                m_Aiming = true;
            }
            //Some actions need to happen when user lifts the finger
            if (Input.GetMouseButtonUp(0))
            {
                //Calculations must stop, so I stop aiming
                m_Aiming = false;
                //To minimize the missclicks Shoot should happen only if power is enough
                if (Mathf.Abs(TrajectoryManager.Instance.Power) > 1f)
                {
                    //Playing the throw sound
                    AudioManager.Instance.PlayThrowSound();
                    //Shoots the Projectile
                    GameManager.Instance.PlayerShootsProjectile(TrajectoryManager.Instance.ForceVector);
                    //Moves to the next State
                    GameManager.Instance.SetState(GameState.PlayerShoots);
                }
                //I stop showing the trajectory
                TrajectoryManager.Instance.ShowTrajectory(false);
                //When release finger reset calculate angle and power
                TrajectoryManager.Instance.ResetPowerAndAngle();
            }
        }
    }

    #endregion

    //-----------------------------------------------------------------


}
