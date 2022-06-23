using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newObjectData", menuName = "Bang's Things/ScriptObjects/ObjectData")]
public class ObjectData : ScriptableObject
{
    public List<CustomData> datas= new List<CustomData>();

    public void AddVariable()
    {
        datas.Add(new CustomData());
    }
    public void DeletVariable(string name)
    {
        foreach(var data in datas)
        {
            if (data.name == name)
            {
                datas.RemoveAt(datas.IndexOf(data));
                break;
            }
        }
    }
    public CustomData GetData(string name)
    {
        foreach(CustomData data in datas)
        {
            if(data.name == name)
                return data;
        }
        return null;
    }
}


[System.Serializable]
public class CustomData
{
    public string name;
    public DataType dataType;

    public string string_v;
    public int int_v;
    public float float_v;
    public Vector2 vector2_v;
    public Vector3 vector3_v;
    public bool bool_v;
    
    public dynamic GetValue<T>()
    {
        if ((typeof(T) == typeof(string)) && (dataType == DataType.String))
            return string_v;
        else if((typeof(T) == typeof(int)) && (dataType == DataType.Integer))
            return int_v;
        else if( (typeof(T) == typeof(float)) && (dataType == DataType.Float))
            return float_v;
        else if((typeof(T) == typeof(Vector2)) && (dataType == DataType.Vector2))
            return vector2_v;
        else if((typeof(T) == typeof(Vector3)) && (dataType == DataType.Vector3))
            return vector3_v;
        else if((typeof(T) == typeof(bool)) && (dataType == DataType.Boolen))
            return bool_v;
        return "WrongType.";
    }
}
public enum DataType
{
    String, Integer, Float, Vector2, Vector3, Boolen
}