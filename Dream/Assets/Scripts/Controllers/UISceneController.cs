using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISceneController : MonoBehaviour
{
    public Button Options;
    public Button NewGame;
    public Button Quit;

    void Start()
    {
        Options.onClick.AddListener(() => FindObjectOfType<GameManager>().transform.Find("Canvas").Find("OptionsUI").gameObject.SetActive(true));
        NewGame.onClick.AddListener(() => FindObjectOfType<ScenesManager>().StartGame());
        Quit.onClick.AddListener(() => FindObjectOfType<ScenesManager>().QuitGame());
    }
}
