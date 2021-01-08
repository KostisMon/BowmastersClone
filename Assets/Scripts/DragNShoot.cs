using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DragNShoot : MonoBehaviour
{
    private Vector2 m_Direction, m_SnapPos;
    private bool m_Aiming;
    private float m_Angle;
    private float m_Force=5f;

    public GameObject PointPrefab;
    public GameObject[] Points;

    public int m_NumberOfPoints;

    private void Start()
    {

        Points = new GameObject[m_NumberOfPoints];
        for (int i = 0; i < m_NumberOfPoints; i++)
        {
            Points[i] = Instantiate(PointPrefab, transform.position, Quaternion.identity);
        }
          
    }

    private void Update()
    {
        if (m_Aiming)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            m_Angle = (mousePos.y- m_SnapPos.y  );
            Vector2 bow = transform.position;
            m_Direction = mousePos - bow;
            //if (m_Angle > Mathf.PI)
            //{
            //    m_Angle = Mathf.PI;
            //}
            m_Force = Mathf.Abs((bow.x - mousePos.x) );
            //if (m_Force > 20)
            //    m_Force = 20;
            ////Camera.main.orthographicSize = (m_Force / 8f) ;
            //Vector2 bowPos = transform.position;


            FaceMouse();
            for (int i = 0; i < m_NumberOfPoints; i++)
            {
                Points[i].transform.position = PointPosition(i * 0.1f);
            }
        }


        if (!m_Aiming && Input.GetMouseButtonDown(0))
        {
            m_SnapPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            m_Aiming = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            m_Aiming = false;
            //Shoot();
        }
    }

    void Shoot()
    {
        Rigidbody2D arrowRigidBody = GetComponent<Rigidbody2D>();

        arrowRigidBody.velocity = new Vector2(Mathf.Cos(m_Angle) * m_Force, Mathf.Sin(m_Angle) * m_Force);
        arrowRigidBody.simulated = true;
    }

    void FaceMouse()
    {
        transform.right = m_Direction;
    }

    Vector2 PointPosition(float t)
    {
        Vector2 currentPointPos = (Vector2)transform.position + (m_Direction.normalized * m_Force * t) + 0.5f * Physics2D.gravity * (t * t);
        return currentPointPos;
    }
}
