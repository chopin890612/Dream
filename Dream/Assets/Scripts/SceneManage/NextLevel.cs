using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class NextLevel : MonoBehaviour
{
    public string currentLevel;
    public string nextLevel;
    public PlayableAsset camChange;
    void Start()
    {
        EventManager.instance.LevelChangeEvent.AddListener(LevelChangeHandler);
    }
    void Update()
    {
        
    }
    
    private void LevelChangeHandler(string levelName)
    {
        if (levelName == currentLevel)
        {
            GameManager.instance.DoForSeconds(() => GetComponent<BoxCollider>().enabled = true, 1);
        }
        else
        {
            GameManager.instance.DoForSeconds(() => GetComponent<BoxCollider>().enabled = false, 1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        { 
            //GameManager.instance.CamBorderChange(nextBorder);
            GameManager.instance.ChangeCam(camChange);
            EventManager.instance.LevelChangeEvent.Invoke(nextLevel);
        }
    }

}
