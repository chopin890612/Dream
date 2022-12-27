using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineControl : MonoBehaviour
{
    public PlayableDirector director;
    public PlayableAsset[] playables;
    public bool playOnStart = false;

    [SerializeField]private int index = 0;

    private void Start()
    {
        if (playables.Length > 0 && playOnStart)
        {
            director.playableAsset = playables[index];
            director.Play();
        }
    }
    public void PausePlayable()
    {
        director.Pause();
    }
    public void PlayPlayable()
    {
        director.Play();
    }
    public void ResumePlayable()
    {
        director.Resume();
    }
    public void StopPlayable()
    {
        director.Stop();
    }
    public void PlayNext()
    {
        director.Stop();
        director.playableAsset = playables[++index];
        director.Play();
    }
    public void ChangePlayable(int index)
    {
        director.Stop();
        director.playableAsset = playables[index];
        director.Play();
    }
}
