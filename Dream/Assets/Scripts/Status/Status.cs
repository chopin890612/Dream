using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Status<T>
{
    public T value { get; private set; }
    public Status(T value)
    {
        this.value = value;
    }
    public T ChangeValue(T newvalue)
    {
        return value = newvalue;
    }
}
