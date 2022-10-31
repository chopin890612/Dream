using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public FireSpot[] fireSpots;

    private bool _inRange = false;
    [SerializeField] private int _maxSpotCount;
    [SerializeField] private int _currntSpotCount;
    // Start is called before the first frame update
    void Start()
    {
        _maxSpotCount = fireSpots.Length;
        _currntSpotCount = 0;
        EventManager.instance.CollectFireEvent.AddListener(FireSpotCallback);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FireSpotCallback()
    {
        _currntSpotCount++;
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            _inRange = true;

        Debug.Log("123");
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            _inRange = false;
    }
}
