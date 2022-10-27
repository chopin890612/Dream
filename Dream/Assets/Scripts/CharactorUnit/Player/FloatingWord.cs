using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FloatingWord : MonoBehaviour
{
    public Vector2 multiplier = new Vector2(1,1);
    public GameObject word;
    public Vector2 targetPos;
    public bool isChangWorld  = false;
    public InputAction action;

    private Vector2 wordBase;
    private float timer;
    private int faceDir;
    private WordState wordState = WordState.Initial;
    private enum WordState
    {
        Initial,
        Floating,
        Platforming,
        ResetValue
    }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.ChangeWorldEvent.AddListener(ChangeWorld);
        action.Enable();
        action.performed += ctx => ChangeWorld();
        wordBase = word.transform.position;
        Initial();
    }

    // Update is called once per frame
    void Update()
    {
        switch (wordState)
        {
            case WordState.Initial:
                break;
            case WordState.Floating:
                RandomMove();
                break;
            case WordState.Platforming:
                TransformToPlatform();
                break;
            case WordState.ResetValue:
                Deplatform();
                break;
        }
    }
    private void ChangeWorld()
    {
        if (isChangWorld)
        {
            wordState = WordState.ResetValue;
            isChangWorld = false;
        }
        else
        {
            wordState = WordState.Platforming;
            isChangWorld = true;
        }
    }
    private void Initial()
    {
        targetPos = transform.position;        
        timer = 0;
        float mX = Random.Range(1f, 4f);
        float my = Random.Range(1f, 5f);
        multiplier = new Vector2(mX, my);
        faceDir = Random.Range(0, 100) > 50 ? 1 : -1;
    }
    private void TransformToPlatform()
    {
        Vector2 currentPos = word.transform.position;
        var x = Mathf.Lerp(targetPos.x, currentPos.x, 0.9f);
        var y = Mathf.Lerp(targetPos.y, currentPos.y, 0.9f);
        word.transform.position = new Vector2(x, y);
    }
    private void Deplatform()
    {
        Vector2 currentPos = word.transform.position;
        var x = Mathf.Lerp(wordBase.x, currentPos.x, 0.9f);
        var y = Mathf.Lerp(wordBase.y, currentPos.y, 0.9f);
        word.transform.position = new Vector2(x, y);

        if(Mathf.Abs(wordBase.y - y) < 0.1f) 
        {
            Initial();
            wordState = WordState.Floating;
        }
    }
    private void RandomMove()
    {
        timer += Time.deltaTime;
        Vector2 currentPos = word.transform.position;
        float x = currentPos.x + Time.deltaTime * multiplier.x * faceDir;
        float y = wordBase.y + Mathf.Sin(timer) *  multiplier.y;
        word.transform.position = new Vector2(x, y);
    }

    private void OnTriggerEnter(Collider other)
    {
        ChangeWorld();
    }
    private void OnTriggerExit(Collider other)
    {
        ChangeWorld();
    }
}
