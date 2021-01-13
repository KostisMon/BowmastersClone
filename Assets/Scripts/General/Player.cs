using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //-----------------------------------------------------------------

    #region Variables
    public enum PlayerType
    {
        Player,
        Enemy
    }
    public PlayerType CharType;
    public GameObject LeftArm, RightArm, Body, ProjectileStartPos;
    private float m_MaxHealth =15f;
    private float m_CurHealth;
    private EnemyShootBehaviour m_EnemyBehaviour;

    #endregion

    //-----------------------------------------------------------------

    #region Public Methods
    //used to apply damage to player/enemy
    public void TakeDamage(float damage)
    {
        //decreasing health
        m_CurHealth -= damage;
        //update the health ui
        UiManager.Instance.InGameMenu.UpdateCharHealth(CharType, m_CurHealth, m_MaxHealth);

        //if health is below zero then depending of the PlayerType player lose or win
        if (m_CurHealth <= 0f)
        {
            m_CurHealth = 0f;

            if (CharType == PlayerType.Player)
            {
                GameManager.Instance.EnemyWon();
            }
            else if (CharType == PlayerType.Enemy)
            {
                GameManager.Instance.PlayerWon();
            }
        }
        else
        {
            //if health above 0 checking the next state
            GameManager.Instance.CheckStateAfterHit();
        }
    }

    //used for arm rotation based on mouse position
    public void ArmsFollow(Vector3 folloPos)
    {
        Vector3 mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(folloPos);
        Vector2 dir = new Vector2(
        mousePosition.x - LeftArm.transform.position.x,
        mousePosition.y - LeftArm.transform.position.y
        );

        RightArm.transform.up = dir;

        LeftArm.transform.up = dir;
    }

    //refreshing current health to max
    public void MaxHealth()
    {
        m_CurHealth = m_MaxHealth;
    }

    #endregion

    //-----------------------------------------------------------------

    #region Private Methods
    private void Start()
    {
        MaxHealth();
        //if enemy then add shoot behaviour script
        //pass the player script to EnemyShootBehaviour
        //add event to StateChang event
        if (CharType == PlayerType.Enemy) 
        {
            m_EnemyBehaviour = gameObject.AddComponent<EnemyShootBehaviour>();
            m_EnemyBehaviour.PlayerScript = this;
            GameManager.Instance.OnStateChange += CheckState;
        }
    }
    
    //I needed some time before the shoot sequence begins so I used Invoke
    private void CheckState()
    {
        GameState curState = GameManager.Instance.GetState();
        switch (curState)
        {
            case GameState.EnemyAims:
                Invoke("StartEnemySequence", 1.5f);
                break;
        }
    }

    //starting enemy shoot sequence
    private void StartEnemySequence()
    {
        m_EnemyBehaviour.StartShootSequence();
    }
    #endregion

    //-----------------------------------------------------------------

}
