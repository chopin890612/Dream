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
            GameManager.instance.DoForSeconds(() => GetComponent<BoxCollider>().enabled = true, (float)camChange.duration);
        }
        else
        {
            GameManager.instance.DoForSeconds(() => GetComponent<BoxCollider>().enabled = false, (float)camChange.duration);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        { 
            //GameManager.instance.CamBorderChange(nextBorder);
            GameObject.FindObjectOfType<DirectorController>().ChangeCam(camChange);
            EventManager.instance.LevelChangeEvent.Invoke(nextLevel);
            GetComponent<BoxCollider>().enabled = false;
        }
    }

}
