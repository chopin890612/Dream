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

    public GameObject SelectBox;
    public GameObject SelectPrefab;

    /// <summary>
    /// 訊息文字顯示
    /// </summary>
    public GameObject MessageBanner;
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

    public List<GameObject> selectObjects;
    private int selectIndex = 0;
    
    private bool isPlaying;
    private bool onSelect;

    private void Start()
    {
        EventManager.instance.TalkToNPCEvent.AddListener(StartConversation);
        DialogueBox.SetActive(false);
    }
    public void StartConversation(DialogueData conversation)
    {
        this.dialogueData = conversation;

        DialogueBox.SetActive(true);
        contextIndex = 0;
        sentenceIndex = 0;

        StartSentence();
        InputHandler.instance.OnSelect += NextSentence;
        InputHandler.instance.OnPressUp += SelectUp;
        InputHandler.instance.OnPressDown += SelectDown;

    }
    public void EndConversation()
    {
        InputHandler.instance.OnSelect -= NextSentence;
        InputHandler.instance.OnPressUp -= SelectUp;
        InputHandler.instance.OnPressDown -= SelectDown;

        GameManager.instance.ChangeGameState(GameManager.GameState.GameView);
    }

    private void StartSentence()
    {
        SetContext();        
    }
    private void SelectUp(InputHandler.InputArgs args)
    {
        if (onSelect)
        {
            selectIndex--;
            if (selectIndex < 0)
                selectIndex = 0;
            ShowSelectOutline();
        }
    }
    private void SelectDown(InputHandler.InputArgs args)
    {
        if (onSelect)
        {
            selectIndex++;
            if (selectIndex > selectObjects.Count - 1)
                selectIndex = selectObjects.Count - 1;
            ShowSelectOutline();
        }
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
            foreach(var select in selectObjects)
            {
                Destroy(select);
            }

            List<GameObject> tempList = new List<GameObject>();
            foreach(var select in currentContext.selects)
            {
                var tempSelect = Instantiate(SelectPrefab, SelectBox.transform);
                tempSelect.GetComponentInChildren<Text>().text = select;
                tempList.Add(tempSelect);
            }
            selectObjects = tempList;

            selectIndex = 0;
            ShowSelectOutline();

            MessageTxt.text = "";

            SelectBox.SetActive(true);
            MessageBanner.SetActive(false);

            onSelect = true;
        }
        else
        {
            onSelect = false;

            SelectBox.SetActive(false);
            MessageBanner.SetActive(true);

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
    private void ShowSelectOutline()
    {
        foreach(var select in selectObjects)
        {
            select.GetComponentInChildren<RawImage>().enabled = false;
        }
        selectObjects[selectIndex].GetComponentInChildren<RawImage>().enabled = true;
    }
}
