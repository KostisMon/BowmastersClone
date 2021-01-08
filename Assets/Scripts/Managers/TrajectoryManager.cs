using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryManager : Singleton<TrajectoryManager>
{
    //-----------------------------------------------------------------

    #region Variables
    private float m_TimeStamp;
    private Vector2 m_TrajectoryPointPos;
    Transform[] m_TrajectoryPointsList;
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
            TrajectoryUpdate(GameManager.Instance.TrajectoryStartPos.transform.position, InputManager.Instance.Force);
        }
    }

    public void ShowTrajectory(bool show)
    {
        GameManager.Instance.TrajectoryParent.SetActive(show);
    }

    public void TrajectoryUpdate(Vector2 trajectoryStartPos, Vector2 force)
    {
        m_TimeStamp = GameManager.Instance.TrajectoryPointSpacing;
        for (int i = 0; i < m_TrajectoryPointsList.Length; i++)
        {
            m_TrajectoryPointPos.x = (trajectoryStartPos.x + force.x * m_TimeStamp);
            m_TrajectoryPointPos.y = (trajectoryStartPos.y + force.y * m_TimeStamp) - (Physics2D.gravity.magnitude * (m_TimeStamp * m_TimeStamp)) / 2f;

            m_TrajectoryPointsList[i].position = m_TrajectoryPointPos;
            m_TimeStamp += GameManager.Instance.TrajectoryPointSpacing;
        }
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

    Vector2 PointPosition(float t)
    {
        Vector2 currentPointPos = (Vector2)GameManager.Instance.TrajectoryStartPos.transform.position + (InputManager.Instance.Direction.normalized * (InputManager.Instance.Force *2)* t) + 0.5f * Physics2D.gravity * (t * t);
        return currentPointPos;
    }
    #endregion

    //-----------------------------------------------------------------
}
