using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TriggerPlay : MonoBehaviour
{
    public PlayableDirector director;
    public int playablesIndex;
    public bool changePlayable = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(!changePlayable)
                director.Resume();
            else
                director.GetComponent<TimelineControl>().ChangePlayable(playablesIndex);
        }
    }
}
