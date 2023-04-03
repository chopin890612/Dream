using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggPlayer : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField] float inputM;
    private Rigidbody2D rb;
    private float rotation = 0;

    [SerializeField] private bool canControl = false;

    private void Start()
    {        
        rb = GetComponent<Rigidbody2D>();
    }
    public void SetControl(bool enable)
    {
        canControl = enable;
    }

    private void Update()
    {
        inputM = InputHandler.instance.PMovment.x * -1f;

        if (canControl)
        {            
            //rotation += inputM * speed;
        }
    }
    private void FixedUpdate()
    {
        if (canControl)
        {
            rb.AddTorque(inputM * speed);
            
        }
    }
}
