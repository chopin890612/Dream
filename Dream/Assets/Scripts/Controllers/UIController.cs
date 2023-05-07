using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static GameManager;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class UIController : MonoBehaviour
{
    public GameObject PauseUI;
    public GameObject SettingContent;
    public Button BackButton;

    [SerializeField] private int pauseIndex = 0;
    [SerializeField] private int settingIndex = 0;
    [SerializeField] private UIState currentState = UIState.None;

    /// <summary>
    /// 0: KeyBoard, 1: Mouse, 2: GamePad
    /// </summary>
    public InputDevice[] inputD = new InputDevice[3];

    [SerializeField] double kbLastUpdateTime;
    [SerializeField] double mouseLastUpdateTime;
    [SerializeField] double gamepadLastUpdateTime;

    private GameObject[] pauseSelections;
    private GameObject[] settingSelections;
    private bool isPressed = false;
    public bool isMainMenu = false;
    public enum UIState
    {
        Pause = 0,
        Setting = 1,
        None = 100
    }
    private void Start()
    {
        pauseSelections = new GameObject[PauseUI.transform.childCount];
        InputHandler.instance.OnUIConfirm += ctx => Select();
        InputHandler.instance.OnUIBack += ctx => Back();
        InputHandler.instance.OnUIPause += ctx => Back();
        InputHandler.instance.OnPause += ctx => OnPause();

        for(int i = 0; i < pauseSelections.Length; i++)
        {
            pauseSelections[i] = PauseUI.transform.GetChild(i).GetChild(1).gameObject;
        }
        settingSelections = new GameObject[SettingContent.transform.childCount];
        for(int i = 0; i< settingSelections.Length; i++)
        {
            settingSelections[i] = SettingContent.transform.GetChild(i).gameObject;
        }

        foreach(var device in InputSystem.devices)
        {
            if(device is Keyboard)
            {
                Debug.Log(device.name);
                inputD[0] = device;
            }
            else if(device is Mouse)
            {
                inputD[1] = device;
            }
            else if(device is Gamepad)
            {
                inputD[2] = device;
            }
        }
    }
    private void Update()
    {
        MoveIndex();
        SelectionUpdate();

        kbLastUpdateTime = inputD[0].lastUpdateTime;
        mouseLastUpdateTime = inputD[1].lastUpdateTime;
        gamepadLastUpdateTime = inputD[2].lastUpdateTime;

        
    }
    private void MoveIndex()
    {
        if(InputHandler.instance.UIMovment.y < 0 && !isPressed)
        {
            AddIndex();
            isPressed = true;
        }
        else if(InputHandler.instance.UIMovment.y > 0 && !isPressed)
        {
            SubIndex();
            isPressed = true;
        }
        else if(InputHandler.instance.UIMovment.y == 0 && isPressed)
        {
            isPressed = false;
        }
    }
    private void AddIndex()
    {
        switch (currentState)
        {
            case UIState.Pause:
                pauseIndex++;
                break;
            case UIState.Setting:
                settingIndex++;
                break;
            default:
                Debug.LogError($"Wrong UIState: {currentState}.");
                break;
        }
        if(pauseIndex > pauseSelections.Length-1)
        {
            pauseIndex--;
        }
        if(settingIndex > settingSelections.Length - 1)
        {
            settingIndex--;
        }
    }
    private void SubIndex()
    {
        switch (currentState)
        {
            case UIState.Pause:
                pauseIndex--;
                break;
            case UIState.Setting:
                settingIndex--;
                break;
            default:
                Debug.LogError($"Wrong UIState: {currentState}.");
                break;
        }
        if (pauseIndex < 0 )
        {
            pauseIndex++;
        }
        if(settingIndex < 0)
        {
            settingIndex++;
        }
    }

    private void SelectionUpdate()
    {
        if (MainInput() != "Mouse")
        {
            if (currentState == UIState.Pause)
            {
                for (int i = 0; i < pauseSelections.Length; i++)
                {
                    if (pauseIndex == i)
                    {
                        //pauseSelections[i].SetActive(true);
                        PauseUI.transform.GetChild(i).GetComponent<Button>().Select();
                    }
                    else
                    {
                        //pauseSelections[i].SetActive(false);
                        //PauseUI.transform.GetChild(i).GetComponent<Button>().OnPointerExit(null);
                    }
                }
            }
            else if (currentState == UIState.Setting)
            {
                for (int i = 0; i < settingSelections.Length; i++)
                {
                    if (settingIndex == i)
                    {
                        //settingSelections[i].transform.GetChild(0).gameObject.SetActive(true);
                        settingSelections[i].transform.GetChild(1).GetComponent<Slider>().Select();
                    }
                    else
                    {
                        //settingSelections[i].transform.GetChild(0).gameObject.SetActive(false);
                    }
                }
            }
        }
    }
    private void Select()
    {
        if (currentState == UIState.Pause)
        {
            pauseSelections[pauseIndex].GetComponentInParent<Button>().onClick.Invoke();
        }
        else if(currentState == UIState.Setting)
        {

        }
    }
    public void Back()
    {
        if (currentState == UIState.Pause)
        {
            PauseUI.SetActive(false);
            instance.ChangeGameState(instance.previousState);
            SetUIState(UIState.None);
        }
        else if (currentState == UIState.Setting && !isMainMenu)
        {
            BackButton.onClick.Invoke();
            SetUIState(UIState.Pause);
        }
        else if(currentState == UIState.Setting && isMainMenu)
        {
            BackButton.onClick.Invoke();
            SetUIState(UIState.None);
            isMainMenu = false;
        }
    }
    private void OnPause()
    {
        if (!PauseUI.activeSelf)
        {
            PauseUI.SetActive(true);
            instance.ChangeGameState(GameState.GameMenu);
            SetUIState(UIState.Pause);
        }
    }
    public string MainInput()
    {
        int Bigindex = 0;
        for(int i = 0; i < inputD.Length; i++)
        {
            if (inputD[i].lastUpdateTime > inputD[Bigindex].lastUpdateTime)
            {
                Bigindex = i;
            }
        }
        switch (Bigindex)
        {
            case 0:
                return "KeyBord";
            case 1:
                return "Mouse";
            case 2:
                return "GamePad";
        }
        return "";
    }
    public void SetUIState(UIState state)
    {
        currentState = state;
    }
    /// <param name="state">0: PauseUI, 1: SettingUI, 100: None</param>
    public void SetUIState(int state)
    {
        currentState = (UIState)state;
    }
}
