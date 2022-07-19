using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{    // Update is called once per frame
    public Vector3 Move;

    void Update()
    {
        Move = InputHandler.instance.Movement;
        //transform.Translate(InputHandler.instance.Movement * Time.deltaTime);
        GetComponent<Rigidbody>().velocity = Move;
    }
}
