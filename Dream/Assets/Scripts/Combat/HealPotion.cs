using System.Collections;
using UnityEngine;

public class HealPotion : MonoBehaviour
{
    public float healValue = 10f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Body")) 
        {

        }
    }
}
