using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayTimeline : MonoBehaviour
{
    public PlayableDirector director;
    public PlayableAsset playable;
    private bool canPlay = false;
    void Start()
    {
        InputHandler.instance.OnInteract += ctx => PlayDirector();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlayDirector()
    {
        if (canPlay)
        {
            Debug.Log("PlayDirector");
            director.playableAsset = playable;
            director.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canPlay = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canPlay = false;
        }
    }
}
