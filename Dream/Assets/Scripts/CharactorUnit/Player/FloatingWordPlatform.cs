using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingWordPlatform : MonoBehaviour
{
    public FloatingWord[] words;

    private BoxCollider platform;
    // Start is called before the first frame update
    void Start()
    {
        platform = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        EnablePlatform();
    }

    private void EnablePlatform()
    {
        int wordCount = 0;
        foreach(var word in words)
        {
            if (word.isChangWorld)
            {
                wordCount++;
            }
        }

        if(wordCount > words.Length/2)
            platform.enabled = true;
        else
            platform.enabled = false;
    }
}
