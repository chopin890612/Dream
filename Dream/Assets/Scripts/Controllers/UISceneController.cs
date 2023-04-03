using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISceneController : MonoBehaviour
{
    public Button Options;

    void Start()
    {
        Options.onClick.AddListener(() => FindObjectOfType<GameManager>().transform.Find("Canvas").Find("OptionsUI").gameObject.SetActive(true));
    }
}
