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
    public bool enableRandom = true;
    public bool enableChageWorld = true;
    public bool wordCollider = false;
    public InputAction action;
    public Vector2 wordBorder = new Vector2(1f,1f);

    private Vector2 wordBase;
    private float timer;
    private int faceDir;
    [SerializeField] private WordState wordState = WordState.Initial;

    [SerializeField] private Vector2 posi;
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
        EventManager.instance.EndChangeWorldEvent.AddListener(EndChageWorldCallback);
        action.Enable();
        action.performed += ctx => ChangeWorld();        
        wordBase = word.transform.position;
        if (wordCollider)
            word.GetComponent<BoxCollider>().enabled = true;
        else
            word.GetComponent<BoxCollider>().enabled = false;
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
                if (wordCollider)
                    word.GetComponent<BoxCollider>().enabled = true;
                else
                    word.GetComponent<BoxCollider>().enabled = false;
                break;
            case WordState.Platforming:
                TransformToPlatform();
                if (wordCollider)
                    word.GetComponent<BoxCollider>().enabled = false;
                break;
            case WordState.ResetValue:
                Deplatform();                
                break;
        }    
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, wordBorder);
    }
    private void ChangeWorld()
    {
        if (enableChageWorld)
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
    }
    private void EndChageWorldCallback()
    {
        if (isChangWorld)
        {
            wordState = WordState.ResetValue;
            isChangWorld = false;
        }
    }
    private void Initial()
    {
        targetPos = transform.position;        
        timer = 0;
        var orgPosition = word.transform.position;
        if (enableRandom)
            wordBase = new Vector2(orgPosition.x, Random.Range(orgPosition.y + 1f, orgPosition.y - 1f));
        float mX = Random.Range(1f, 4f);
        float my = Random.Range(1f, 5f);
        if(enableRandom)
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
        if (InBorder() == false)
            faceDir *= -1;
        timer += Time.deltaTime;
        Vector2 currentPos = word.transform.position;
        float x = currentPos.x + Time.deltaTime * multiplier.x * faceDir;
        float y = wordBase.y + Mathf.Sin(timer) *  multiplier.y;
        word.transform.position = new Vector2(x, y);
    }
    private bool InBorder()
    {
        Vector2 pos = word.transform.position;
        posi = transform.position;
        if(pos.x < transform.position.x - wordBorder.x / 2f ||
           pos.x > transform.position.x + wordBorder.x / 2f)
        {
            return false;
        }
        else
            return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        ChangeWorld();
        Debug.Log(other);
    }
    private void OnTriggerExit(Collider other)
    {
        ChangeWorld();
    }
}
