using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //-----------------------------------------------------------------

    #region Variables
    private Rigidbody2D m_RigidBody;
    private CircleCollider2D m_CircleCollider;
    #endregion

    //-----------------------------------------------------------------

    #region Private Methods
    private void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();
        m_CircleCollider = GetComponent<CircleCollider2D>();
    }
    #endregion

    //-----------------------------------------------------------------

    #region Public Methods
    public void Push(Vector2 force)
    {
        m_RigidBody.AddForce(force, ForceMode2D.Impulse);
    }

    public void ActivateRb()
    {
        m_RigidBody.isKinematic = false;
    }

    public void DesactivateRb()
    {
        m_RigidBody.velocity = Vector3.zero;
        m_RigidBody.angularVelocity = 0f;
        m_RigidBody.isKinematic = true;
    }
    #endregion

    //-----------------------------------------------------------------


}
