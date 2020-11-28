using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine.UI;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Linq;
using JetBrains.Annotations;
using System;

[CustomEditor(typeof(PressableButton))]
public class MethodListDrawer : Editor
{
    public override void OnInspectorGUI()
    {   
        base.OnInspectorGUI();

        PressableButton scr = (PressableButton)target;
                
        var methods = scr.effected.GetComponent(Type.GetType(scr.className)).GetType().GetMethods();
        List<string> mNames = new List<string>();
        foreach (var m in methods)
        {
            mNames.Add(m.Name);
        }
        GUIContent label = new GUIContent("Methods");
        scr.selected = EditorGUILayout.Popup(label, scr.selected, mNames.ToArray());

    }
}
