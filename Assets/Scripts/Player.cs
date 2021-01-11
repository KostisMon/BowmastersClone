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
    public GameObject LeftArm, RightArm;
    private float m_MaxHealth =15f;
    private float m_CurHealth;


    #endregion

    //-----------------------------------------------------------------

    #region Public Methods

    public void TakeDamage(float damage)
    {
        m_CurHealth -= damage;
        switch (CharType)
        {
            case PlayerType.Player:
                UiManager.Instance.InGameMenu.UpdatePlayerHealth(m_CurHealth, m_MaxHealth);
                break;
            case PlayerType.Enemy:
                UiManager.Instance.InGameMenu.UpdateEnemyHealth(m_CurHealth, m_MaxHealth);
                break;
            default:
                break;
        }

        if (m_CurHealth <= 0f)
        {
            m_CurHealth = 0f;

            if (CharType == PlayerType.Player)
            {
                GameManager.Instance.PlayerWon();
            }
            else if (CharType == PlayerType.Enemy)
            {
                GameManager.Instance.EnemyWon();
            }
        }
    }

    public void ArmsFollow(Vector3 mousePos)
    {
        Vector3 mousePosition ;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 dir = new Vector2(
        mousePosition.x - LeftArm.transform.position.x,
        mousePosition.y - LeftArm.transform.position.y
        );
       
        RightArm.transform.up = dir;

        LeftArm.transform.up = dir;
    }

    

    #endregion

    //-----------------------------------------------------------------

    #region Private Methods
    private void Start()
    {
        m_CurHealth = m_MaxHealth;
    }

    #endregion

    //-----------------------------------------------------------------

}
