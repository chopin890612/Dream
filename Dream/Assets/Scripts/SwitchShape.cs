using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SwitchShape : MonoBehaviour
{
    [SerializeField] GameObject Human;
    [SerializeField] GameObject Snake;
    [SerializeField] ParticleSystem pp;
    [SerializeField] GameObject chatWindow;
    [SerializeField] bool isBeingSnake = true;
    [SerializeField] Transform checkPoint;
    private InputMaster _inputAction;

    private bool _dashButton;
    private bool _npcCanTalk = false;


    void Start()
    {
        _inputAction = new InputMaster();
        _inputAction.Enable();
        EventManager.eventManager.SwitchShapeEvent.AddListener(ShapeShift) ;
        EventManager.eventManager.PlayerDeadEvent.AddListener(PlayerDeadCallback);
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
        GameManager.instance.SetControllerVibration(0.5f, 0.3f, 0.2f);
    }

    private void Dash(InputAction.CallbackContext context)
    {
        if (_npcCanTalk && isBeingSnake)
        {
            if (chatWindow.activeSelf == false)
                chatWindow.SetActive(true);
            else
                chatWindow.SetActive(false);
        }
        else
        {
            chatWindow.SetActive(false);
        }
    }

    private void ShapeShift(bool isSnake)
    {
        Human.SetActive(!isSnake);
        Snake.SetActive(isSnake);
        pp.Play();
    }
    private void PlayerDeadCallback()
    {
        transform.position = checkPoint.position;
        GameManager.instance.SetControllerVibration(0.8f, 0.7f, 0.5f);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter.");
        if (other.CompareTag("NPC"))
        {
            _npcCanTalk = true;
            chatWindow.GetComponentInChildren<Text>().text = other.gameObject.name;
        }
        if (other.CompareTag("Trap"))
        {
            EventManager.eventManager.PlayerDeadEvent.Invoke();
        }
        if (other.CompareTag("CheckPoint"))
        {
            checkPoint = other.transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger Exit.");
        if (other.CompareTag("NPC"))
        {
            chatWindow.SetActive(false);
            _npcCanTalk = false;
        }
    }
}
