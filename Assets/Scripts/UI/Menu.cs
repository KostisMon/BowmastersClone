using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Menu : MonoBehaviour
{
    //-------------------------------------------------------------------------
    
    #region Variables
    public static Menu CurrentMenu;
    public static int MenusLoaded;
    #endregion

    //-------------------------------------------------------------------------

    #region Public Virtual Methods

    public virtual void Update()
    {

    }

    public virtual void Start()
    {

    }

    public virtual void Awake()
    {
        MenusLoaded++;
        if (CurrentMenu == null)
        {
            CurrentMenu = this;
        }
    }

    #endregion

    //-------------------------------------------------------------------------

    #region Public Methods

    public void Show(bool show)
    {
        gameObject.SetActive(show);
    }

    public void SetMethod(Button button, UnityAction action)
    {
        if (button == null)
        {
            return;
        }
        //Remove the existing events
        button.onClick.RemoveAllListeners();
        //Add your new event
        button.onClick.AddListener(action);
    }

    public void SetMethod(Toggle button, UnityAction<bool> action)
    {
        //Remove the existing events
        button.onValueChanged.RemoveAllListeners();
        //Add your new event
        button.onValueChanged.AddListener(action);
    }

    #endregion

    //-------------------------------------------------------------------------


}
