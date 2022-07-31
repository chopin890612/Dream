#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AutoScale))]
public class AutoScaleEditor : Editor
{
    AutoScale _target;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        _target = (AutoScale)target;

        

        float Ratio = FrustumHeight(_target.transform.position.z) / FrustumHeight(_target.PlayerDeepth);

        _target.transform.localScale = new Vector3(_target.BaseScale.x * Ratio, _target.BaseScale.y * Ratio, 1);

        GUILayout.Label(Ratio.ToString());
    }
    private float FrustumHeight(float distance)
    {
        float _distance = distance - Camera.main.transform.position.z;
        float frustumHeight = 2.0f * _distance * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        return frustumHeight;
    }
}
#endif