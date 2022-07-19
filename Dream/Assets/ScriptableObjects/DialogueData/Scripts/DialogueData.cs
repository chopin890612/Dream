using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newConversation", menuName = "Bang's Things/ScriptObjects/DialogueData/DialogueData")]
public class DialogueData : ScriptableObject
{
    [SerializeField]
    public List<DialogueContext> data = new List<DialogueContext>();
}

[System.Serializable]
public class DialogueContext
{
    public Sprite charImage;
    [Range(0,1)]public int charSide; // 0 �b����, 1�b�k��
    public string charName;

    [TextArea]
    public List<string> sentence = new List<string>();
}