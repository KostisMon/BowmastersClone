using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//General Manager for Projectile Trajectory management
public class TrajectoryManager : Singleton<TrajectoryManager>
{
    //-----------------------------------------------------------------

    #region Variables
    private float m_TimeStamp;
    private Vector2 m_TrajectoryPointPos;
    private Transform[] m_TrajectoryPointsList;
    private float m_Distance;
    public float Distance
    {
        get { return m_Distance; }
    }
    private float m_Power;
    public float Power
    {
        get { return m_Power; }
    }
    private float m_Angle;
    public float Angle
    {
        get { return m_Angle; }
    }
    private Vector2 m_Direction;
    private Vector2 m_ForceVector;
    public Vector2 ForceVector
    {
        get { return m_ForceVector; }
    }
    #endregion

    //-----------------------------------------------------------------

    #region Public Methods

    //hides trajectory
    //initializes trajectory
    public void Init()
    {
        ShowTrajectory(false);
        InitializeTrajectory();
    }

    public void Update()
    {
        //here I used the Aiming attribute and not the State
        //because I needed the update to happen after touch
        if (InputManager.Instance.Aiming)
        {
            TrajectoryUpdate(GameManager.Instance.TrajectoryStartPos.transform.position, m_ForceVector);
        }
    }

    //Calculating trajectory based on start and end point of touch
    public void CalculateTrajectory(Vector2 startPoint, Vector2 endPoint)
    {
        //calculating power
        CalculatePower(startPoint, endPoint);
        //calculating angle
        CalculateAngle(startPoint, endPoint);
        //calculating direction based on angle
        CalculateDirection(m_Angle);
        //calculating force vector based on direction and power
        CalculateForceVector(m_Direction, m_Power);

        //showing trajectory only when power >0
        if (!GameManager.Instance.TrajectoryParent.activeSelf)
        {
            if (m_Power > 0f)
            {
                ShowTrajectory(true);               
            }
        }
        
        
    }

    //used to show/hide trajectory
    public void ShowTrajectory(bool show)
    {
        GameManager.Instance.TrajectoryParent.SetActive(show);
    }

    //used to reset power and angle
    public void ResetPowerAndAngle()
    {
        m_Power = 0f;
        m_Angle = 0f;
    }

    //updating trajectory while player aims
    //setting the position for each point
    // math is base on trajectory motion Math (https://en.wikipedia.org/wiki/Projectile_motion)
    public void TrajectoryUpdate(Vector2 trajectoryStartPos, Vector2 force)
    {
        m_TimeStamp = GameManager.Instance.TrajectoryPointSpacing;
        for (int i = 0; i < m_TrajectoryPointsList.Length; i++)
        {
            m_TrajectoryPointPos.x = (trajectoryStartPos.x + (force.x * (m_TimeStamp)));
            m_TrajectoryPointPos.y = ((trajectoryStartPos.y + force.y * m_TimeStamp) - (Physics2D.gravity.magnitude * m_TimeStamp * m_TimeStamp) / 2f);

            m_TrajectoryPointsList[i].position = m_TrajectoryPointPos;
            m_TimeStamp += GameManager.Instance.TrajectoryPointSpacing;
        }
    }

    #endregion

    //-----------------------------------------------------------------

    #region Private Methods
    //Initialises the trajectory system
    void InitializeTrajectory()
    {
        //Creating and populating a list of points
        m_TrajectoryPointsList = new Transform[GameManager.Instance.TrajectoryPointCount];
        //scaling them depending on the input from Gamemanager
        GameManager.Instance.TrajectoryPoint.transform.localScale = Vector3.one * GameManager.Instance.TrajectoryPointMaxScale;
        
        //decreasing each point's scale 
        float trajectoryPointScale = GameManager.Instance.TrajectoryPointMaxScale;
        float trajectoryPointScaleFactor = trajectoryPointScale / m_TrajectoryPointsList.Length;

        //instantiating the points and parenting them for tidyness
        for (int i = 0; i < m_TrajectoryPointsList.Length; i++)
        {
            m_TrajectoryPointsList[i] = GameObject.Instantiate(GameManager.Instance.TrajectoryPoint, null).transform;
            m_TrajectoryPointsList[i].parent = GameManager.Instance.TrajectoryParent.transform;

            m_TrajectoryPointsList[i].localScale = Vector3.one * trajectoryPointScale;
            if (trajectoryPointScale > GameManager.Instance.TrajectoryPointMinScale)
            {
                trajectoryPointScale -= trajectoryPointScaleFactor;
            }
        }
    }


    //calculate power
    private void CalculatePower(Vector2 startPos, Vector2 endPos)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(endPos);
        mousePos.z = 0;
        Vector3 start = Camera.main.ScreenToWorldPoint(startPos);
        start.z = 0;

        m_Distance = Vector3.Distance(mousePos, start);
        m_Power = m_Distance * GameManager.Instance.PowerMultiplier;
        //clamping power to 30 (found this after testing)
        if (m_Power > 30)
        {
            m_Power = 30;
        }
    }
    
    //calculating angle
    private void CalculateAngle(Vector2 startPos, Vector2 endPos)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(endPos);
        mousePos.z = 0;
        Vector3 start = Camera.main.ScreenToWorldPoint(startPos);
        start.z = 0;

        Vector3 dir = mousePos - start;
        m_Angle = Vector2.SignedAngle(dir, GameManager.Instance.Player.Body.transform.right);
        m_Angle = m_Angle >= 0 ? 180 - m_Angle : -(180 + m_Angle);

    }

    //calculating Force vector
    private void CalculateForceVector(Vector2 direction, float power)
    {
        m_ForceVector = direction * power;
    }

    //calculating direction
    private void CalculateDirection(float angle)
    {
        m_Direction = new Vector2((float)Mathf.Cos((angle) * Mathf.Deg2Rad),
                           (float)Mathf.Sin((angle) * Mathf.Deg2Rad));
    }


    #endregion

    //-----------------------------------------------------------------
}
