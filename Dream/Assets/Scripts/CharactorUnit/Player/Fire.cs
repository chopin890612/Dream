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
        foreach(FireSpot fireSpot in fireSpots)
        {
            fireSpot.gameObject.SetActive(false);
        }
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

            _currntSpotCount = 0;
            StartCoroutine(ShowFireSpot());
        }
    }

    private void FireSpotCallback(FireSpot spot)
    {
        if (spot.spotIndex == _currntSpotCount)
        {
            _currntSpotCount++;
            EventManager.instance.FireDashEvent.Invoke();            
        }

        spot.gameObject.SetActive(false);
    }

    private IEnumerator ShowFireSpot()
    {
        for(int i = 0; i < _maxSpotCount; i++)
        {
            fireSpots[i].gameObject.SetActive(true);
            fireSpots[i].spotIndex = i;

            yield return new WaitForSeconds(1f);
        }
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
