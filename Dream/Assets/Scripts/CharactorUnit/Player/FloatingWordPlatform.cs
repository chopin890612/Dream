using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingWordPlatform : MonoBehaviour
{
    public bool isParticalPlatform = false;
    public GameObject image;
    public FloatingWord[] words;
    public ChangeWorldDetect[] particalPlatforms;

    private BoxCollider[] platform;
    private ParticleSystemForceField field;
    // Start is called before the first frame update
    void Start()
    {
        platform = GetComponents<BoxCollider>();
        foreach(BoxCollider collider in platform)
        {
            collider.enabled = false;
        }
        if (isParticalPlatform)
        {
            field = GetComponent<ParticleSystemForceField>();
            field.enabled = false;
        }
        if(image != null) image.SetActive(false);
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

        if (wordCount >= words.Length / 2f)
        {
            foreach (BoxCollider collider in platform)
            {
                collider.enabled = true;
            }
            if (image != null) image.SetActive(true);
        }
        else
        {
            foreach (BoxCollider collider in platform)
            {
                collider.enabled = false;
            }
            if (image != null) image.SetActive(false);
        }
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
            foreach (BoxCollider collider in platform)
            {
                collider.enabled = true;
            }
            field.enabled = true;
            if (image != null) image.SetActive(true);
        }
        else
        {
            foreach (BoxCollider collider in platform)
            {
                collider.enabled = false;
            }
            field.enabled = false;
            if (image != null) image.SetActive(false);
        }
    }
    
}
