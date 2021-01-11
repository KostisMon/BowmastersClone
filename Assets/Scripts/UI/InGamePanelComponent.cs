using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGamePanelComponent : MenuComponent
{
    //-----------------------------------------------------------------

    #region Variables

    private TextMeshProUGUI m_PowerText, m_AngleText, m_CharDistanceText;
    private GameObject m_IndicatorRightPos, m_IndicatorLeftPos, m_TrajectoryUi, m_CharDistanceUi, m_PlayerDistanceIcon, m_EnemyDistanceIcon, m_PlayerDistanceArrow, m_EnemyDistanceArrow;
    private Image m_PlayerHealthImage, m_EnemyHealthImage;

    #endregion

    //-----------------------------------------------------------------

    #region Public Methods

    public InGamePanelComponent(Menu menu)
    {
        p_ParentMenu = menu;
        PanelObj = menu.gameObject.transform.Find("InGamePanelComponent").gameObject;
        RectTransform[] trans = PanelObj.GetComponentsInChildren<RectTransform>();
        for (int i = 0; i < trans.Length; i++)
        {
            if (trans[i].name == "TrajectoryUi")
            {
                m_TrajectoryUi = trans[i].gameObject;
            }
            if (trans[i].name == "IndicatorRightPos")
            {
                m_IndicatorRightPos = trans[i].gameObject;
            }
            if (trans[i].name == "IndicatorLeftPos")
            {
                m_IndicatorLeftPos = trans[i].gameObject;
            }
            if (trans[i].name == "CharIcon")
            {
                m_CharDistanceUi = trans[i].gameObject;
            }
            if (trans[i].name == "PowerAmmount")
            {
                m_PowerText = trans[i].GetComponent<TextMeshProUGUI>();
            }
            if (trans[i].name == "AngleAmmount")
            {
                m_AngleText = trans[i].GetComponent<TextMeshProUGUI>();
            }
            if (trans[i].name == "CharDistanceText ")
            {
                m_CharDistanceText = trans[i].GetComponent<TextMeshProUGUI>();
            }
            if (trans[i].name == "PlayerArrow")
            {
                m_PlayerDistanceArrow = trans[i].gameObject;
            }
            if (trans[i].name == "EnemyArrow")
            {
                m_EnemyDistanceArrow = trans[i].gameObject;
            }
            if (trans[i].name == "PlayerIcon")
            {
                m_PlayerDistanceIcon = trans[i].gameObject;
            }
            if (trans[i].name == "EnemyIcon")
            {
                m_EnemyDistanceIcon = trans[i].gameObject;
            }
            if (trans[i].name == "PlayerHealthImage")
            {
                m_PlayerHealthImage = trans[i].GetComponent<Image>();
            }
            if (trans[i].name == "EnemyHealthImage")
            {
                m_EnemyHealthImage = trans[i].GetComponent<Image>();
            }

        }
    }

    public void UpdatePlayerHeath(float curHealth, float maxHealth)
    {
        m_PlayerHealthImage.fillAmount = curHealth / maxHealth;
    }

    public void UpdateEnemyHeath(float curHealth, float maxHealth)
    {
        m_EnemyHealthImage.fillAmount = curHealth / maxHealth;

    }
    
    public void UpdatePlayerDistance(float distance)
    {
        m_CharDistanceText.text = Mathf.RoundToInt(distance).ToString() + " m";
    }

    public void UpdateDistanceUiPos()
    {
        RectTransform rect = m_CharDistanceUi.GetComponent<RectTransform>();
        Vector3 target = Vector3.zero;
        if (GameManager.Instance.GetState() == GameState.PlayerAims || GameManager.Instance.GetState() == GameState.PlayerShoots)
        {
            target = Camera.main.WorldToScreenPoint(GameManager.Instance.Enemy.GetComponentInChildren<Collider2D>().transform.position);
        }
        else if (GameManager.Instance.GetState() == GameState.EnemyAims || GameManager.Instance.GetState() == GameState.EnemyShoots)
        {
            target = Camera.main.WorldToScreenPoint(GameManager.Instance.Player.GetComponentInChildren<Collider2D>().transform.position);
        }
        float clampPosy = target.y;
        clampPosy = Mathf.Clamp(clampPosy, rect.rect.height / 1.5f, Screen.height - rect.rect.height / 1.5f);

        m_CharDistanceUi.transform.position = new Vector3(m_CharDistanceUi.transform.position.x, clampPosy, m_CharDistanceUi.transform.position.z);
        
    }

    public void InitialiseDistanceUi()
    {
        GameState curState = GameManager.Instance.GetState();
        switch (curState)
        {
            case GameState.PlayerAims:
                m_CharDistanceUi.transform.position = m_IndicatorRightPos.transform.position;
                PrepareDistanceUi(true);
                break;
            case GameState.PlayerShoots:
                break;
            case GameState.EnemyAims:
                m_CharDistanceUi.transform.position = m_IndicatorLeftPos.transform.position;
                PrepareDistanceUi(false);
                break;
            case GameState.EnemyShoots:
                break;

        }   
    }

    public void ShowCharDistanceUi(bool show)
    {
        m_CharDistanceUi.SetActive(show);
    }

    public void ShowTrajectoryInfo(bool show)
    {
        m_TrajectoryUi.SetActive(show);
    }

    public void SetPowerText(float power)
    {
        float powerScaled = UiManager.Instance.FloatScale(0f, 30f, 0f, 100f, power);
        m_PowerText.text = powerScaled.ToString("F2");
    }

    public void SetAngleText(float angle)
    {
        m_AngleText.text = Mathf.RoundToInt(angle).ToString() + "°";
    }

    #endregion

    //-----------------------------------------------------------------

    #region Private Methods

    private void PrepareDistanceUi(bool forPlayer)
    {
        m_EnemyDistanceArrow.SetActive(forPlayer);
        m_PlayerDistanceArrow.SetActive(!forPlayer);
        m_EnemyDistanceIcon.SetActive(forPlayer);
        m_PlayerDistanceIcon.SetActive(!forPlayer);
    }
    #endregion

    //-----------------------------------------------------------------

}
