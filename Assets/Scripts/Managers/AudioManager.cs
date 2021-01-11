using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    //-----------------------------------------------------------------

    #region Variables

    AudioSource m_AudioSource;

    #endregion

    //-----------------------------------------------------------------

    #region Public Methods

    public void Init()
    {
        m_AudioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();
    }


    public void PlayThrowSound()
    {
        m_AudioSource.PlayOneShot(GameManager.Instance.ThrowSound);
    }

    public void PlayHitSound()
    {
        m_AudioSource.PlayOneShot(GameManager.Instance.HitSound);
    }
    #endregion

    //-----------------------------------------------------------------


}
