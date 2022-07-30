using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OpenDoor : MonoBehaviour
{
    public Animator animator;

    public bool isOpened = false;

    public bool canOpen;

    public Cinemachine.CinemachineVirtualCamera virtualCamera;

    private void Update()
    {
        if(canOpen && InputHandler.instance.Movement.y > 0.5f)
        {
            if (isOpened == true)
                StartCoroutine(Close());
            else if (isOpened == false)
                StartCoroutine(Open());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            canOpen = true;
            virtualCamera.Priority = 11;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canOpen = false;
            virtualCamera.Priority = 9;
        }
    }
    private IEnumerator Open()
    {
        animator.Play("Up", 0);
        isOpened = true;
        canOpen = false;
        yield return new WaitForSeconds(0.5f);
        canOpen = true;
    }
    private IEnumerator Close()
    {
        animator.Play("Down", 0);
        isOpened = false; 
        canOpen = false;
        yield return new WaitForSeconds(0.5f);
        canOpen = true;
    }
}
