using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseObjectPool : MonoBehaviour
{
    public BasePoolObject targetObject;
    public int countLimit = 20;
    private  Queue<GameObject> poolObjects = new Queue<GameObject>();

    private void Awake()
    {
        InitalPool();
    }

    public virtual void Reuse() { }
    public virtual void EndUse() { }


    private void InitalPool()
    {
        for(int i = 0; i < countLimit; i++)
        {
            GameObject go = Instantiate(targetObject.gameObject, this.transform);
            poolObjects.Enqueue(go);
            go.GetComponent<BasePoolObject>().SetPool(this);
            go.SetActive(false);
        }
    }
}
