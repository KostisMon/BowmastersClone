using TMPro;
using UnityEngine;

public class UiManager : Singleton<UiManager>
{

    //-----------------------------------------------------------------

    #region Variables

    private TextMeshProUGUI m_PowerText, m_AngleText;

    #endregion

    //-----------------------------------------------------------------

    #region Public Methods

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
    #endregion

    //-----------------------------------------------------------------
}
