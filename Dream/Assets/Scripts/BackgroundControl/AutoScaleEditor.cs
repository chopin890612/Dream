#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AutoScale))]
public class AutoScaleEditor : Editor
{
    AutoScale _target;

    private Vector2 BaseAngle;
    private Vector2 angleRatio;
    private float Ratio;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //GUI.enabled = false;

        _target = (AutoScale)target;


        BaseAngle = BasicAngle(_target.DefaultScreenSize);
        angleRatio = new Vector2(Mathf.Sin(BaseAngle.x) * Mathf.Abs(_target.transform.position.z - Camera.main.transform.position.z),
                                 Mathf.Sin(BaseAngle.y) * Mathf.Abs(_target.transform.position.z - Camera.main.transform.position.z));
        Ratio = Mathf.Abs(_target.transform.position.z - Camera.main.transform.position.z) / Mathf.Abs(_target.PlayerDeepth - Camera.main.transform.position.z);

        _target.transform.localScale = new Vector3(_target.BaseScale.x * Ratio, _target.BaseScale.y * Ratio, 1);
        //_target.transform.localScale = new Vector3(_target.BaseScale.x * angleRatio.x, _target.BaseScale.y * angleRatio.y, 1);

        GUILayout.Label("BaseAngle: " + new Vector2(BaseAngle.x * Mathf.Rad2Deg, BaseAngle.y * Mathf.Rad2Deg).ToString());
        GUILayout.Label("AngleRatio: " + angleRatio.ToString());
        GUILayout.Label("ScreenRatio: " + new Vector2(1920f / _target.DefaultScreenSize.x, 1080f / _target.DefaultScreenSize.y));
        GUILayout.Label(Ratio.ToString());
    }
    private Vector2 BasicAngle(Vector2 baseSize)
    {
        return new Vector2(Mathf.Atan(baseSize.x / 200f / Mathf.Abs(_target.PlayerDeepth - Camera.main.transform.position.z)), 
                            Mathf.Atan(baseSize.y / 200f / Mathf.Abs(_target.PlayerDeepth - Camera.main.transform.position.z)));
    }
}
#endif