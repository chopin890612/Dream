using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchShape : MonoBehaviour
{
    [SerializeField] GameObject Human;
    [SerializeField] GameObject Snake;
    [SerializeField] ParticleSystem pp;
    [SerializeField] GameObject chatWindow;
    [SerializeField] bool isBeingSnake = true;
    private InputMaster _inputAction;

    private bool _dashButton;
    private bool _npcCanTalk = false;


    void Start()
    {
        _inputAction = new InputMaster();
        _inputAction.Enable();
        EventManager.eventManager.SwitchShapeEvent.AddListener(ShapeShift) ;
        _inputAction.Player.ShapeShift.started += SwitchShapeEvent;
        _inputAction.Player.Dash.started += Dash;
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void SwitchShapeEvent(InputAction.CallbackContext context)
    {
        isBeingSnake = !isBeingSnake;
        EventManager.eventManager.SwitchShapeEvent.Invoke(isBeingSnake);
    }

    private void Dash(InputAction.CallbackContext context)
    {
        if (_npcCanTalk)
        {
            if (chatWindow.activeSelf == false)
                chatWindow.SetActive(true);
            else
                chatWindow.SetActive(false);
        }
    }

    private void ShapeShift(bool isSnake)
    {
        Human.SetActive(!isSnake);
        Snake.SetActive(isSnake);
        pp.Play();
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter.");
        if(other.gameObject.CompareTag("NPC"))
            _npcCanTalk = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            chatWindow.SetActive(false);
            _npcCanTalk = false;
        }
    }
}
