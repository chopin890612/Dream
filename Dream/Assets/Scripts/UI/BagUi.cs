using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BagUi : MonoBehaviour
{
    public int index = 1;
    public string[] bagPageNames = new string[3];
    public GameObject[] bagPageContents = new GameObject[3];
    public TMP_Text title;
    public TMP_Text previous;
    public TMP_Text next;
    public GameObject pre;
    public GameObject nex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TextPrevious()
    {
        if (--index <= 0)
        {
            index = 0;
            previous.text = "No Previous";
            pre.SetActive(false);
        }
        else
        {
            pre.SetActive(true);
            nex.SetActive(true);
            previous.text = bagPageNames[index - 1];
        }
        title.text = bagPageNames[index];
        next.text = bagPageNames[index + 1];
        ShowContent();
    }
    public void TextNext()
    {
        if (++index >= bagPageNames.Length -1)
        {
            index = bagPageNames.Length - 1;
            next.text = "No Next";
            nex.SetActive(false);
        }
        else
        {
            pre.SetActive(true);
            nex.SetActive(true);
            next.text = bagPageNames[index + 1];
        }
        title.text = bagPageNames[index];
        previous.text = bagPageNames[index - 1];
        ShowContent();
    }
    private void ShowContent()
    {
        for(int i = 0; i < bagPageContents.Length; i++)
        {
            if(index == i)
            {
                bagPageContents[i].SetActive(true);
            }
            else
            {
                bagPageContents[i].SetActive(false);
            }
        }
    }
}
