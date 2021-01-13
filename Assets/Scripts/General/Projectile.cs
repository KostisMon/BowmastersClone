using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //-----------------------------------------------------------------

    #region Variables
    private Rigidbody2D m_RigidBody;
    private Collider2D m_Collider2D;
    [Range(1, 7)] public int Damage;
    #endregion

    //-----------------------------------------------------------------

    #region Private Methods
    //fetching rigidbody and collider
    private void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();
        m_Collider2D = GetComponent<Collider2D>();
    }

    //Changing rotation based on velocity
    private void Update()
    {
        Vector2 vel = m_RigidBody.velocity;
        float angle = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    //different states of projectile collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //we dont want to play sound when projectile goes out of map
        if (!collision.CompareTag("Border"))
        {
            AudioManager.Instance.PlayHitSound();
        }
         
        //taking damage from the player or the enemy
        if (collision.CompareTag("Player")  )
        {
            GameManager.Instance.Player.TakeDamage(Damage);
        }
        else if (collision.CompareTag("Enemy"))
        {
            GameManager.Instance.Enemy.TakeDamage(Damage);
        }
        //if collision happen with the ground we move to the next state
        else if (collision.CompareTag("Ground"))
        {
            if (GameManager.Instance.GetState() == GameState.PlayerShoots)
            {
                GameManager.Instance.SetState(GameState.EnemyAims);
            }
            else if (GameManager.Instance.GetState() == GameState.EnemyShoots)
            {
                GameManager.Instance.SetState(GameState.PlayerAims);
            }
        }
        //same with the borders, moving to the next state but returing to avoid further projectile behaviour
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
        else if (collision.CompareTag("Limbs"))
        {
            return;
        }
        //when collision happen projectile "digs" a bit inside
        transform.position += new Vector3(0, -0.15f); 
        //freezes and stops simulating Physics
        m_RigidBody.velocity = new Vector2(0, 0);
        m_RigidBody.simulated = false;
        //Also destroying rigidbody collider and projectile script
        Destroy(gameObject.GetComponent<Rigidbody2D>());
        Destroy(gameObject.GetComponent<Collider2D>());
        Destroy(this);
    }

    #endregion

    //-----------------------------------------------------------------

    #region Public Methods
    //used to apply force upo shooting
    public void Shoot(Vector2 force)
    {
        m_RigidBody.AddForce(force, ForceMode2D.Impulse);
    }

    #endregion

    //-----------------------------------------------------------------


}
