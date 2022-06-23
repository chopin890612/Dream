using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(ObjectData))]
public class ObjectDataEditor : Editor
{
    private ObjectData data;

    private string deletName;
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        data = (ObjectData)target;

        if (data.datas.Count == 0)
            data.datas.Add(new CustomData());


        GUILayout.BeginVertical();
        foreach(CustomData data in this.data.datas)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Name: ", GUILayout.Width(70));
            data.name = EditorGUILayout.TextField(data.name);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Value: ", GUILayout.Width(70));
            data.dataType = (DataType)EditorGUILayout.EnumPopup(data.dataType, GUILayout.Width(100));
            DisplayType(data);
            GUILayout.EndHorizontal();
            GUILayout.Space(20);

        }
        GUILayout.EndVertical();

        if (GUILayout.Button("NewData", GUILayout.Height(30)))
        {
            data.AddVariable();
        }

        GUILayout.BeginHorizontal();
        GUILayout.Label("DeletName: ", GUILayout.Width(70));
        deletName = EditorGUILayout.TextField(deletName);
        if (GUILayout.Button("Delet"))
        {
            data.DeletVariable(deletName);
            deletName = "";
        }
        GUILayout.EndHorizontal();
    }
    private void DisplayType(CustomData data)
    {
        switch (data.dataType)
        {
            case DataType.String:
                data.string_v = EditorGUILayout.DelayedTextField(data.string_v);
                break;
            case DataType.Integer:
                data.int_v = EditorGUILayout.DelayedIntField(data.int_v);
                break;
            case DataType.Float:
                data.float_v = EditorGUILayout.DelayedFloatField(data.float_v);
                break;
            case DataType.Vector2:
                data.vector2_v = EditorGUILayout.Vector2Field("",data.vector2_v);
                break;
            case DataType.Vector3:
                data.vector3_v = EditorGUILayout.Vector3Field("", data.vector3_v);
                break;
            case DataType.Boolen:
                data.bool_v = EditorGUILayout.Toggle(data.bool_v);
                break;
            default:
                Debug.LogWarning("Wrong datatype " + data.dataType);
                break;
        }
    }
}
