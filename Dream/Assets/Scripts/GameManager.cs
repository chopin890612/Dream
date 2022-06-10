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

    public void WaitForSeconds(float seconds)
    {
        StartCoroutine(WaitSecondsCoroutine(seconds));          
    }
    private IEnumerator WaitSecondsCoroutine(float seconds)
    {
        for(float i = 0; i < seconds; i+= Time.deltaTime)
            yield return 0;
    }
}
