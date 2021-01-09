using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void Init()
    {
        ShowTrajectory(false);
        InitializeTrajectory();
        
    }

    public void FixedUpdate()
    {
        if (InputManager.Instance.Aiming)
        {
            TrajectoryUpdate(GameManager.Instance.TrajectoryStartPos.transform.position, m_ForceVector);
        }
    }

    public void CalculateTrajectory(Vector2 startPoint, Vector2 endPoint)
    {
        CalculatePower(startPoint, endPoint);
        CalculateAngle(startPoint, endPoint);
        CalculateDirection(m_Angle);
        CalculateForceVector(m_Direction, m_Power);

        if (!GameManager.Instance.TrajectoryParent.activeSelf)
        {
            if (m_Power > 2f)
            {
                ShowTrajectory(true);
            }
        }
        if (m_Power > 2f)
        {
            UiManager.Instance.SetPowerAndAngleText(m_Power, m_Angle);
            UiManager.Instance.ShowPowerAndAngleUi(true);
        }
        
    }

    public void ShowTrajectory(bool show)
    {
        GameManager.Instance.TrajectoryParent.SetActive(show);
    }

    
    #endregion

    //-----------------------------------------------------------------

    #region Private Methods
    void InitializeTrajectory()
    {
        m_TrajectoryPointsList = new Transform[GameManager.Instance.TrajectoryPointCount];

        GameManager.Instance.TrajectoryPoint.transform.localScale = Vector3.one * GameManager.Instance.TrajectoryPointMaxScale;

        float trajectoryPointScale = GameManager.Instance.TrajectoryPointMaxScale;
        float trajectoryPointScaleFactor = trajectoryPointScale / m_TrajectoryPointsList.Length;

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

    //calculate power
    private void CalculatePower(Vector2 startPos, Vector2 endPos)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(endPos);
        mousePos.z = 0;
        Vector3 start = Camera.main.ScreenToWorldPoint(startPos);
        start.z = 0;

        m_Distance = Vector3.Distance(mousePos, start);
        m_Power = m_Distance * GameManager.Instance.PowerMultiplier;
        if (m_Power > 30)
        {
            m_Power = 30;
        }

        //1st way
        //Vector2 vectorA = new Vector2(endPos.x - startPos.x, endPos.y - startPos.y);
        //float currentDistance = Vector2.SqrMagnitude(vectorA);
        //currentDistance = Mathf.Sqrt(currentDistance);
        //m_Power = currentDistance;

        //if (m_Power > 20)
        //{
        //    m_Power = 20;
        //}
        //m_Angle = Vector2.SignedAngle(vectorA, GameManager.Instance.Player.transform.right);
        //m_Angle = m_Angle >= 0 ? 180 - m_Angle : -(180 + m_Angle);
    }

    private void CalculateAngle(Vector2 startPos, Vector2 endPos)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(endPos);
        mousePos.z = 0;
        Vector3 start = Camera.main.ScreenToWorldPoint(startPos);
        start.z = 0;

        Vector3 dir = mousePos - start;
        m_Angle = Vector2.SignedAngle(dir, GameManager.Instance.LeftHand.transform.parent.transform.right);
        m_Angle = m_Angle >= 0 ? 180 - m_Angle : -(180 + m_Angle);

    }


    private void CalculateForceVector(Vector2 direction, float power)
    {
        m_ForceVector = direction * power;
    }

    private void CalculateDirection(float angle)
    {
        m_Direction = new Vector2((float)Mathf.Cos((angle) * Mathf.Deg2Rad),
                           (float)Mathf.Sin((angle) * Mathf.Deg2Rad));
    }


    #endregion

    //-----------------------------------------------------------------
}
