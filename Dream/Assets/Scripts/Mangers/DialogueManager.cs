using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    /// <summary>
    /// 欲顯示的對話框物件
    /// </summary>
    public GameObject DialogueBox;

    /// <summary>
    /// 訊息文字顯示
    /// </summary>
    public Text MessageTxt;

    /// <summary>
    /// 說話角色名稱
    /// </summary>
    public Text CharNameTxt0;
    public GameObject CharNameImage0;
    public Text CharNameTxt1;
    public GameObject CharNameImage1;

    /// <summary>
    /// 說話角色頭像
    /// </summary>
    public Image CharImg0;
    public Image CharImg1;

    public DialogueData dialogueData;

    private int contextIndex = 0;
    private DialogueContext currentContext;

    private int sentenceIndex = 0;
    private string currentSentence;
    
    private bool isPlaying;

    public void StartConversation(DialogueData conversation)
    {
        this.dialogueData = conversation;

        DialogueBox.SetActive(true);

        StartSentence();
        InputHandler.instance.OnSelect += ctx =>NextSentence(ctx);

    }
    public void EndConversation()
    {
        InputHandler.instance.OnSelect -= ctx => NextSentence(ctx);
    }

    private void StartSentence()
    {
        SetContext();

        MessageTxt.text = currentSentence;

        if (currentContext.charSide == 0)
        {
            CharImg0.sprite = currentContext.charImage;

            CharImg0.gameObject.SetActive(true);
            CharImg1.gameObject.SetActive(false);

            CharNameTxt0.text = currentContext.charName;
            CharNameImage0.SetActive(true);

            CharNameImage1.SetActive(false);
        }
        else if (currentContext.charSide == 1)
        {
            CharImg1.sprite = currentContext.charImage;

            CharImg1.gameObject.SetActive(true);
            CharImg0.gameObject.SetActive(false);

            CharNameTxt1.text = currentContext.charName;
            CharNameImage1.SetActive(true);

            CharNameImage0.SetActive(false);
        }
    }
    private void NextSentence(InputHandler.InputArgs args)
    {
        if (isPlaying)
            return;
        else
        {
            sentenceIndex++;
            if(sentenceIndex < currentContext.sentence.Count)
            {

            }
            else
            {

            }
        }
    }
    private void SetContext()
    {
        currentContext = dialogueData.data[contextIndex];
        currentSentence = currentContext.sentence[sentenceIndex];        
    }

}
