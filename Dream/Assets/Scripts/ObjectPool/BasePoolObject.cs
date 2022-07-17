using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePoolObject : MonoBehaviour
{
    public UnityEvent enactiveEvent;
    public BaseObjectPool pool { get; private set; }
    
    public void SetPool(BaseObjectPool pool)
    {
        this.pool = pool;
    }
}
