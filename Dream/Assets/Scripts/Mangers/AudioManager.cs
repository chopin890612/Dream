using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] BGMS;
    private AudioSource m_AudioSource;

    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        switch (scene.buildIndex)
        {
            case 1:
                m_AudioSource.clip = BGMS[0];
                m_AudioSource.Play();
                break;
            case 2:
                m_AudioSource.clip = BGMS[1];
                m_AudioSource.Play();
                break;
            case 3:
                m_AudioSource.clip = BGMS[2];
                m_AudioSource.Play();
                break;
            case 4:
                m_AudioSource.clip = BGMS[3];
                m_AudioSource.Play();
                break;
            case 5:
                m_AudioSource.clip = BGMS[4];
                m_AudioSource.Play();
                break;
            case 6:
                m_AudioSource.clip = BGMS[5];
                m_AudioSource.Play();
                break;
        }
    }
}
