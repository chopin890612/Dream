using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeText : MonoBehaviour
{
    public int index = 0;
    public List<string> texts;
    public TMP_Text textMeshPro;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PreviousSelect()
    {        
        if(--index < texts.Count-1)
        {
            index = 0;
        }
        textMeshPro.text = texts[index];
    }
    public void NextSelect()
    {
        if (++index > texts.Count-1)
        {
            index = texts.Count-1;
        }
        textMeshPro.text = texts[index];
    }
}
