using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdPlatform : MonoBehaviour
{
    public GameObject Platfrom;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.BirdPlatformEvent.AddListener(EnablePlatform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void EnablePlatform()
    {
        Platfrom.SetActive(true);
    }
}
