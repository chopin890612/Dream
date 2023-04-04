using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public float masterVol;
    public float BGMVol;
    public float SFXVol;

    public UnityEngine.Audio.AudioMixer masterMixer;
    public UnityEngine.Audio.AudioMixerGroup BGMMixer;
    public UnityEngine.Audio.AudioMixerGroup SFXMixer;
    public bool MuteBGM = false;
    public bool MuteSFX = false;
    public static SoundManager instance { get; private set; }
    public AudioClip[] BGM;
    public AudioClip[] SFX;

    GameObject BGMPlayer;
    GameObject SFXPlayer;
    AudioSource BGMSource;
    AudioSource[] SFXSources;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        BGMPlayer = new GameObject("BGMPlayer");
        BGMPlayer.transform.parent = this.transform;
        BGMSource = BGMPlayer.AddComponent<AudioSource>();
        BGMSource.outputAudioMixerGroup = BGMMixer;
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
            tSource.outputAudioMixerGroup = SFXMixer;
            SFXSources[i] = tSource;
        }

        SetupMixerVolume();
    }
    private void Update()
    {
        if (MuteBGM)
            masterMixer.SetFloat("BGMVolume", -80f);

        if (MuteSFX)
            masterMixer.SetFloat("SFXVolume", -80f);
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

    public void OnSliderValueChange(float sliderRawValue, Slider sliderSender)
    {
        switch (sliderSender.name)
        {
            case "Master":
                masterMixer.SetFloat("MasterVolume", sliderRawValue.Remap(-10f, 10, -80, 10));
                break;
            case "BGM":
                if(MuteBGM)
                    masterMixer.SetFloat("BGMVolume", -80f);
                else
                    masterMixer.SetFloat("BGMVolume", sliderRawValue.Remap(-10f, 10, -60, 10));
                break;
            case "SFX":
                if(MuteSFX)
                    masterMixer.SetFloat("SFXVolume", -80f);
                else
                    masterMixer.SetFloat("SFXVolume", sliderRawValue.Remap(-10f, 10, 0, 20));
                break;
        }
    }
    private void SetupMixerVolume()
    {
        masterMixer.SetFloat("MasterVolume", 3f.Remap(-10f, 10, -80, 10));
        masterMixer.SetFloat("BGMVolume", 5f.Remap(-10f, 10, -60, 10));
        masterMixer.SetFloat("SFXVolume", 5f.Remap(-10f, 10, 0, 20));
    }
}
