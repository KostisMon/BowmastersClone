using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//super simple class to use the method from an Animation Event 
public class ResultHelp : MonoBehaviour
{
    //showing results buttons
    public void ShowResultButtons()
    {
        UiManager.Instance.ResultMenu.ShowResultButtons();  
    }
}
