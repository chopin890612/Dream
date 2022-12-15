using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreLoadScene : MonoBehaviour
{
    public int SceneIndex;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //GameManager.instance.LoadScene(SceneIndex);
        }
    }
}
