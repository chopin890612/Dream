using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    /// <summary>
    /// ����ܪ���ܮت���
    /// </summary>
    public GameObject DialogueBox;

    public GameObject SelectBox;
    public GameObject SelectPrefab;

    /// <summary>
    /// �T����r���
    /// </summary>
    public Text MessageTxt;

    /// <summary>
    /// ���ܨ���W��
    /// </summary>
    public Text CharNameTxt0;
    public GameObject CharNameImage0;
    public Text CharNameTxt1;
    public GameObject CharNameImage1;

    /// <summary>
    /// ���ܨ����Y��
    /// </summary>
    public Image CharImg0;
    public Image CharImg1;

    public DialogueData dialogueData;

    private int contextIndex = 0;
    private DialogueContext currentContext;

    private int sentenceIndex = 0;
    private string currentSentence;
    
    private bool isPlaying;
    private bool onSelect;

    private void Start()
    {
        EventManager.instance.TalkToNPCEvent.AddListener(StartConversation);
        DialogueBox.SetActive(false);
    }
    private void Update()
    {
        if (onSelect)
        {

        }
    }
    public void StartConversation(DialogueData conversation)
    {
        this.dialogueData = conversation;

        DialogueBox.SetActive(true);
        contextIndex = 0;
        sentenceIndex = 0;

        StartSentence();
        InputHandler.instance.OnSelect += NextSentence;

    }
    public void EndConversation()
    {
        InputHandler.instance.OnSelect -= NextSentence;
        GameManager.instance.ChangeGameState(GameManager.GameState.GameView);
    }

    private void StartSentence()
    {
        SetContext();        
    }
    private void NextSentence(InputHandler.InputArgs args)
    {
        if (isPlaying)
            return;
        else
        {
            sentenceIndex++;
            if (sentenceIndex < currentContext.sentence.Count)
            {                
                SetContext();
            }
            else
            {
                contextIndex++;
                if (contextIndex < dialogueData.data.Count)
                {                    
                    sentenceIndex = 0;
                    SetContext();
                }
                else
                {
                    EndConversation();
                    DialogueBox.SetActive(false);
                    EventManager.instance.DialogueEndEvent.Invoke();
                }
            }
        }
    }
    private void SetContext()
    {
        currentContext = dialogueData.data[contextIndex];

        if (currentContext.IsSelect)
        {
            for(int i = 0; i < SelectBox.transform.childCount; i++)
            {
                Destroy(SelectBox.transform.GetChild(i).gameObject);
            }
            for(int i = 0; i < currentContext.selects.Count; i++)
            {
                var select = Instantiate(SelectPrefab, SelectBox.transform);
                select.GetComponentInChildren<Text>().text = currentContext.selects[i];
            }

            MessageTxt.text = "";

            SelectBox.SetActive(true);

        }
        else
        {
            SelectBox.SetActive(false);

            currentSentence = currentContext.sentence[sentenceIndex];

            MessageTxt.text = currentSentence;

            if (currentContext.charSide == 0)
            {
                CharImg0.sprite = currentContext.charImage;

                CharImg0.enabled = true;
                CharImg1.enabled = false;

                CharNameTxt0.text = currentContext.charName;
                CharNameImage0.SetActive(true);

                CharNameImage1.SetActive(false);
            }
            else if (currentContext.charSide == 1)
            {
                CharImg1.sprite = currentContext.charImage;

                CharImg1.enabled = true;
                CharImg0.enabled = false;

                CharNameTxt1.text = currentContext.charName;
                CharNameImage1.SetActive(true);

                CharNameImage0.SetActive(false);
            }
        }
    }

}
