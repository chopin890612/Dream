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
        InputHandler.instance.OnFire += ctx => StartFire();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void StartFire()
    {
        if (_inRange)
        {
            Debug.Log("StartFire");
        }
    }

    private void FireSpotCallback()
    {
        _currntSpotCount++;
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _inRange = true;
            //Debug.Log("In");
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _inRange = false;
            //Debug.Log("Out");
        }        
    }
}
