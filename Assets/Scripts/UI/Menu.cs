using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Menu : MonoBehaviour
{
    //-------------------------------------------------------------------------
    
    #region Variables
    public static Menu CurrentMenu;
    #endregion

    //-------------------------------------------------------------------------

    #region Public Virtual Methods

    public virtual void Update()
    {

    }

    public virtual void Start()
    {

    }

    //Setting Current menu for using it when swapping between menus
    public virtual void Awake()
    {
        if (CurrentMenu == null)
        {
            CurrentMenu = this;
        }
    }

    #endregion

    //-------------------------------------------------------------------------

    #region Public Methods
    //Show/hide menu
    public void Show(bool show)
    {
        gameObject.SetActive(show);
    }

    #endregion

    //-------------------------------------------------------------------------


}
