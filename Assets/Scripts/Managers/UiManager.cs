using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : Singleton<UiManager>
{

    //-----------------------------------------------------------------

    #region Variables

    private TextMeshProUGUI m_PowerText, m_AngleText;
    private Button m_PlayButton;
    
    #endregion

    //-----------------------------------------------------------------

    #region Public Methods
    
    public void Init()
    {
        m_PlayButton = GameManager.Instance.MainMenu.GetComponentInChildren<Button>();
        m_PlayButton.onClick.AddListener(PlayGame);
    }

    public void Update()
    {
        if (InputManager.Instance.Aiming && TrajectoryManager.Instance.Power>0f)
        {
            SetPowerAndAngleText(TrajectoryManager.Instance.Power, TrajectoryManager.Instance.Angle);
            ShowPowerAndAngleUi(true);
        }
    }

    public void ShowPowerAndAngleUi(bool show)
    {
        GameManager.Instance.TrajectoryCanvasObj.SetActive(show);
    }

    public void SetPowerAndAngleText(float power, float angle)
    {        
        float powerScaled = FloatScale(0f, 30f, 0f, 100f, power);
        GameManager.Instance.PowerText.text = powerScaled.ToString("F2");
        GameManager.Instance.AngleText.text = Mathf.RoundToInt(angle).ToString() + "°";
    }

    public void UpdatePlayerHealth(Player.PlayerType type, float curHealth, float maxHealth)
    {
        switch (type)
        {
            case Player.PlayerType.Player:
                GameManager.Instance.PlayerHealthImage.fillAmount = curHealth / maxHealth;
                break;
            case Player.PlayerType.Enemy:
                GameManager.Instance.EnemyHealthImage.fillAmount = curHealth / maxHealth;
                break;
            default:
                break;
        }
    }

    #endregion

    //-----------------------------------------------------------------

    #region Private Methods
    public float FloatScale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {

        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }

    private void PlayGame()
    {
        GameManager.Instance.MainMenu.SetActive(false);

        GameManager.Instance.SetState(GameState.PlayerAims);
    }
    #endregion

    //-----------------------------------------------------------------
}
