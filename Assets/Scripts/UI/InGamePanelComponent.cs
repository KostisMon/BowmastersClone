using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Menu Component used for the in game menus of the game
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
    // Constructor, searching and populating variables
    // Plus setting methods for buttons
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

    //used for updating health depending on the char type
    public void UpdateCharHeath(Player.PlayerType type, float curHealth, float maxHealth)
    {
        switch (type)
        {
            case Player.PlayerType.Player:
                m_PlayerHealthImage.fillAmount = curHealth / maxHealth;

                break;
            case Player.PlayerType.Enemy:
                m_EnemyHealthImage.fillAmount = curHealth / maxHealth;

                break;
            default:
                break;
        }
    }
   
    //Resetting health image fill amounts
    public void MaxHealth()
    {
        m_PlayerHealthImage.fillAmount = 1;
        m_EnemyHealthImage.fillAmount = 1;
    }

    //used for updating  Char distance
    public void UpdatePlayerDistance(float distance)
    {
        m_CharDistanceText.text = Mathf.RoundToInt(distance).ToString() + " m";
    }

    //used for updating the position of the Distance Indicator ui
    public void UpdateDistanceUiPos()
    {
        //fetching the rectTransform of the distance UI
        RectTransform rect = m_CharDistanceUi.GetComponent<RectTransform>();
        Vector3 target = Vector3.zero;
        //if the player aims or shoots the target is the Enemy
        if (GameManager.Instance.GetState() == GameState.PlayerAims || GameManager.Instance.GetState() == GameState.PlayerShoots)
        {
            target = Camera.main.WorldToScreenPoint(GameManager.Instance.Enemy.Body.transform.position);
        }
        //else it is the Player
        else if (GameManager.Instance.GetState() == GameState.EnemyAims || GameManager.Instance.GetState() == GameState.EnemyShoots)
        {
            target = Camera.main.WorldToScreenPoint(GameManager.Instance.Player.Body.transform.position);
        }
        float clampPosy = target.y;
        //clamping Y position to always stay in screen view
        clampPosy = Mathf.Clamp(clampPosy, rect.rect.height / 1.5f, Screen.height - rect.rect.height / 1.5f);
        //finally setting the new clamped position
        m_CharDistanceUi.transform.position = new Vector3(m_CharDistanceUi.transform.position.x, clampPosy, m_CharDistanceUi.transform.position.z);
        
    }

    //initialises the Distance Indicator ui disabling/enabling the appropriate objects based on the GameState
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

    //used to show/hide distance indicator ui
    public void ShowCharDistanceUi(bool show)
    {
        m_CharDistanceUi.SetActive(show);
    }

    //used to show/hide trajectory info
    public void ShowTrajectoryInfo(bool show)
    {
        m_TrajectoryUi.SetActive(show);
    }

    //used to update the power text
    public void SetPowerText(float power)
    {
        float powerScaled = GameManager.Instance.FloatScale(0f, 30f, 0f, 100f, power);
        m_PowerText.text = powerScaled.ToString("F2");
    }

    //used to update the angle text
    public void SetAngleText(float angle)
    {
        m_AngleText.text = Mathf.RoundToInt(angle).ToString() + "°";
    }

    #endregion

    //-----------------------------------------------------------------

    #region Private Methods
    //used as an initialization method for the distance indicator ui based on who plays (Player/Enemy)
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
