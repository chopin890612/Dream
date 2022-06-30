using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public Transform fatherPlatform;
    public List<Transform> snakePlatform = new List<Transform>();
    public List<Transform> humanPlatform= new List<Transform>();
    public List<Transform> platforms = new List<Transform>();
    public List<Transform> outlines = new List<Transform>();

    void Start()
    {
        //EventManager.eventManager.SwitchShapeEvent.AddListener(HumanPlatformEnable);
        //EventManager.eventManager.SwitchShapeEvent.AddListener(SnakePlatformEnable);
        EventManager.eventManager.SwitchShapeEvent.AddListener(OutlineEnable);
        
        AssignPlatforms(fatherPlatform);

    }

    void Update()
    {
        
    }
    private void OutlineEnable(bool isSnake)
    {
        foreach(Transform go in outlines)
        {
            go.gameObject.SetActive(isSnake);
        }
    }
    private void SnakePlatformEnable(bool isSnake)
    {
        foreach(Transform pf in snakePlatform)
        {
            pf.gameObject.SetActive(isSnake);
        }
    }
    private void HumanPlatformEnable(bool isSnake)
    {
        foreach (Transform pf in humanPlatform)
        {
            pf.gameObject.SetActive(!isSnake);
        }
    }
    private void AssignPlatforms(Transform father)
    {
        for (int i = 0; i < father.childCount; i++)
        {
            if (father.GetChild(i).CompareTag("Platform"))
            {
                platforms.Add(father.transform.GetChild(i));
            }
            else if (father.transform.GetChild(i).CompareTag("SnakePlatform"))
            {
                snakePlatform.Add(father.transform.GetChild(i));
            }
            else if (father.transform.GetChild(i).CompareTag("HumanPlatform"))
            {
                humanPlatform.Add(father.transform.GetChild(i));
            }
            else if (father.transform.GetChild(i).CompareTag("PlatformGroup"))
            {
                AssignPlatforms(father.transform.GetChild(i));
            }
        }
        for (int i = 0; i < platforms.Count; i++)
        {
            for (int j = 0; j < platforms[i].transform.childCount; j++)
            {
                if (platforms[i].transform.GetChild(j).CompareTag("PlatformOutline"))
                {
                    outlines.Add(platforms[i].transform.GetChild(j));
                }
            }
        }
    }
}
