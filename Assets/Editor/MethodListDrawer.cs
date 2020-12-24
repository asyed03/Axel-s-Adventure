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

        if (scr.effectedItems.Length == 0 || scr.effectedItems[0] == null)
        {
            return;
        }
        var methods = scr.effectedItems[0].GetComponent(scr.className).GetType().GetMethods();
        List<string> mNames = new List<string>();
        foreach (var m in methods)
        {
            mNames.Add(m.Name);
        }
        GUIContent label = new GUIContent("OnButtonDown");
        scr.selected = EditorGUILayout.Popup(label, scr.selected, mNames.ToArray());

        GUIContent label2 = new GUIContent("OnButtonUp");
        scr.selected2 = EditorGUILayout.Popup(label2, scr.selected2, mNames.ToArray());
    }
}
