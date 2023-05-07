using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISceneController : MonoBehaviour
{
    public Button[] selections;
    private int index = 0;
    public UIController uiC;
    private bool isPressed = false;
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }
    void Start()
    {
        selections[1].onClick.AddListener(OpenOption);
        selections[0].onClick.AddListener(() => FindObjectOfType<ScenesManager>().StartGame());
        selections[2].onClick.AddListener(() => FindObjectOfType<ScenesManager>().QuitGame());
        uiC = GameManager.instance.uiController;

        InputHandler.instance.OnPConfirm += ctx => Confirm();
    }
    private void Update()
    {
        MoveIndex();
        UpdateSelect();   
    }
    private void OpenOption()
    {
        FindObjectOfType<GameManager>().transform.Find("Canvas").Find("OptionsUI").gameObject.SetActive(true);
        uiC.SetUIState(UIController.UIState.Setting);
        uiC.isMainMenu = true;
    }
    private void UpdateSelect()
    {
        if(uiC.MainInput() != "Mouse")
        {
            for(int i=0; i<selections.Length; i++)
            {
                if(i == index)
                {
                    selections[i].Select();
                }
            }
        }
    }
    private void Confirm()
    {
        selections[index].onClick.Invoke();
    }
    private void MoveIndex()
    {
        Vector2 move = InputHandler.instance.PMovment;
        Debug.Log(move);
        if(move.y>0 && !isPressed)
        {
            IndexSub();
            isPressed = true;
        }
        else if(move.y<0 && !isPressed)
        {
            IndexAdd();
            isPressed = true;
        }
        else if(move.y == 0)
        {
            isPressed = false;
        }
    }
    private void IndexAdd()
    {
        if (index++ > selections.Length - 1)
            index--;
    }
    private void IndexSub()
    {
        if(index-- < 0)
            index++;
    }
}
