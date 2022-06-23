using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public float rawMove { get; private set; }
    public int NormInputX { get; private set; }

    private InputMaster inputActions;


    private void Awake()
    {
        inputActions = new InputMaster();
        inputActions.Enable();
    }
    private void Update()
    {
        rawMove = inputActions.Player.Movment.ReadValue<float>();
        NormInputX = (int)(rawMove * Vector2.right).normalized.x;
    }
}
