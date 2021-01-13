using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//simple Audio Manager
public class AudioManager : Singleton<AudioManager>
{
    //-----------------------------------------------------------------

    #region Variables

    AudioSource m_AudioSource;

    #endregion

    //-----------------------------------------------------------------

    #region Public Methods
    //Initialise fetching the AudioSource
    public void Init()
    {
        m_AudioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();
    }

    //playing Oneshot of throw sound
    public void PlayThrowSound()
    {
        m_AudioSource.PlayOneShot(GameManager.Instance.ThrowSound);
    }

    //playing Oneshot of hit sound
    public void PlayHitSound()
    {
        m_AudioSource.PlayOneShot(GameManager.Instance.HitSound);
    }

    //playing Oneshot of win sound
    public void PlayWinSound()
    {
        m_AudioSource.Stop();
        m_AudioSource.PlayOneShot(GameManager.Instance.WinSound);
    }

    //playing Oneshot of lose sound
    public void PlayLoseSound()
    {
        m_AudioSource.Stop();
        m_AudioSource.PlayOneShot(GameManager.Instance.LoseSound);
    }

    //Plays sounds
    public void Play()
    {
        m_AudioSource.Play();
    }

    //Stops sounds
    public void Stop()
    {
        m_AudioSource.Stop();
    }
    #endregion

    //-----------------------------------------------------------------


}
