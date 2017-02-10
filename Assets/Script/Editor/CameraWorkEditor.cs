using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(CameraWork))]
[System.Serializable]
public class CameraWorkEditor : Editor
{
    


    void OnEnable()
    {
        EditorGUI.BeginChangeCheck();


        CameraWork camWork = (CameraWork)target;

        camWork.camFollow = camWork.gameObject.GetComponent<CamFollow>();

        if (EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(target);

    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();

        if(EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(target);
        }

    }


}
