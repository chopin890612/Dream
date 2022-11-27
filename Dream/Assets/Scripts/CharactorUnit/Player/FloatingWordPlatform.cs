using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingWordPlatform : MonoBehaviour
{
    public bool isParticalPlatform = false;
    public FloatingWord[] words;
    public ChangeWorldDetect[] particalPlatforms;

    private BoxCollider platform;
    private ParticleSystemForceField field;
    // Start is called before the first frame update
    void Start()
    {
        platform = GetComponent<BoxCollider>();
        platform.enabled = false;
        if (isParticalPlatform)
        {
            field = GetComponent<ParticleSystemForceField>();
            field.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isParticalPlatform)
            EnablePlatform();
        else
            EnableWordPlatform();
    }

    private void EnableWordPlatform()
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
    private void EnablePlatform()
    {
        int Count = 0;
        foreach (var platforms in particalPlatforms)
        {
            if (platforms.isChangeWorld)
            {
                Count++;
            }
        }

        if (Count > particalPlatforms.Length / 2)
        {
            platform.enabled = true;
            field.enabled = true;
        }
        else
        {
            platform.enabled = false;
            field.enabled = false;
        }
    }
    
}
