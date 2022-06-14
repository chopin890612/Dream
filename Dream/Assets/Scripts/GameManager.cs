using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WaitForSeconds(float seconds, System.Action callback)
    {
        StartCoroutine(WaitSecondsCoroutine(seconds, callback));          
    }
    private IEnumerator WaitSecondsCoroutine(float seconds, System.Action callback)
    {
        yield return new WaitForSeconds(seconds);
        callback();
    }
}
