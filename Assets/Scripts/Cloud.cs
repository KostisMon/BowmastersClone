using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    //-----------------------------------------------------------------

    #region Variables
    public Transform CloudStartPos;
    private float m_Speed;
    private float m_TempPos;
    #endregion

    //-----------------------------------------------------------------

    #region Private Methods
    void Start()
    {
        m_Speed = Random.Range(4f, 8f);
    }

    void Update()
    {
        m_TempPos = (Time.deltaTime * m_Speed )+ transform.position.x;
        transform.position = new Vector3(m_TempPos, transform.position.y, transform.position.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        transform.position = new Vector3(CloudStartPos.transform.position.x, transform.position.y, transform.position.z);
    }
    #endregion

    //-----------------------------------------------------------------
}
