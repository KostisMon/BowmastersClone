using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //-----------------------------------------------------------------

    #region Variables
    private Rigidbody2D m_RigidBody;
    private Collider2D m_Collider2D;
    #endregion

    //-----------------------------------------------------------------

    #region Private Methods
    private void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();
        m_Collider2D = GetComponent<Collider2D>();
        DisableCollider();
    }

    private void Update()
    {
        
        Vector2 vel = m_RigidBody.velocity;
        float angle = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")  )
        {
            GameManager.Instance.Player.TakeDamage(5f);
        }
        else if (collision.CompareTag("Enemy"))
        {
            GameManager.Instance.Enemy.TakeDamage(5f);
        }
        else if (collision.CompareTag("Border"))
        {
            if (GameManager.Instance.GetState() == GameState.PlayerShoots)
            {
                GameManager.Instance.SetState(GameState.EnemyAims);
            }
            else if (GameManager.Instance.GetState() == GameState.EnemyShoots)
            {
                GameManager.Instance.SetState(GameState.PlayerAims);
            }
            return;
        }
        transform.position += new Vector3(0, -0.15f); 
        m_RigidBody.velocity = new Vector2(0, 0);
        m_RigidBody.simulated = false;
        Destroy(gameObject.GetComponent<Rigidbody2D>());
        Destroy(gameObject.GetComponent<Collider2D>());
        Destroy(this);

        //Debug
        GameManager.Instance.SetState(GameState.PlayerAims);
    }

    private void EnableCollider()
    {
        m_Collider2D.enabled = true;
    }

    private void DisableCollider()
    {
        m_Collider2D.enabled = false;
    }
    #endregion

    //-----------------------------------------------------------------

    #region Public Methods

    public void Shoot(Vector2 force)
    {
        m_RigidBody.AddForce(force, ForceMode2D.Impulse);
        Invoke("EnableCollider", 0.3f);
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
