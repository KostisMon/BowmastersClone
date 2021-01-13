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
    //setting up a random speed
    void Start()
    {
        m_Speed = Random.Range(4f, 8f);
    }

    //applying the speed to a right movement
    void Update()
    {
        m_TempPos = (Time.deltaTime * m_Speed )+ transform.position.x;
        transform.position = new Vector3(m_TempPos, transform.position.y, transform.position.z);
    }

    //when collide with border I move ti back to the other end
    private void OnCollisionEnter2D(Collision2D collision)
    {
        transform.position = new Vector3(CloudStartPos.transform.position.x, transform.position.y, transform.position.z);
    }
    #endregion

    //-----------------------------------------------------------------
}
