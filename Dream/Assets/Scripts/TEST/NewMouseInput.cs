using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMouseInput : MonoBehaviour
{
    private InputMaster inputActions;
    public Vector2 mousePosition;
    // Start is called before the first frame update
    void Start()
    {
        inputActions = new InputMaster();
        inputActions.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = inputActions.Mouse.Postion.ReadValue<Vector2>();
    }
}
