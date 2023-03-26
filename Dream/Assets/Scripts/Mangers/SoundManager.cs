using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public AudioClip[] BGM;
    public AudioClip[] SFX;

    GameObject BGMPlayer;
    GameObject SFXPlayer;
    AudioSource BGMSource;
    AudioSource[] SFXSources;
    void Start()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);
        BGMPlayer = new GameObject("BGMPlayer");
        BGMPlayer.transform.parent = this.transform;
        BGMSource = BGMPlayer.AddComponent<AudioSource>();
        BGMSource.loop = true;
        PlayBGM(0);


        SFXPlayer = new GameObject("SFXPlayer");
        SFXPlayer.transform.parent = this.transform;
        SFXSources = new AudioSource[SFX.Length];
        for(int i = 0; i < SFX.Length; i++)
        {
            AudioSource tSource = SFXPlayer.AddComponent<AudioSource>();
            tSource.loop = false;
            tSource.playOnAwake = false;
            tSource.clip = SFX[i];
            SFXSources[i] = tSource;
        }
    }

    public void PlayBGM(int index)
    {
        BGMSource.clip = BGM[index];
        BGMSource.Play();
    }
    public void PlaySFX(int index)
    {
        SFXSources[index].Play();
    }
}
