using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Level))]
public class LevelEditor : PropertyDrawer
{
    public static GameManager gameManagerEditor { get { return instance; } }
    public static GameManager instance = null;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty sceneName = property.FindPropertyRelative("sceneName");
        SerializedProperty number = property.FindPropertyRelative("number");
        SerializedProperty score = property.FindPropertyRelative("score");
        SerializedProperty time = property.FindPropertyRelative("time");
        SerializedProperty unlocked = property.FindPropertyRelative("unlocked");

        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, label);

        EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, 20), sceneName, new GUIContent("scene"));

        EditorGUI.PropertyField(new Rect(position.x, position.y + 25, position.width, 20), score, new GUIContent("score"));

        EditorGUI.PropertyField(new Rect(position.x, position.y + 55, position.width, 20), time, new GUIContent("time"));

        EditorGUI.PropertyField(new Rect(position.x, position.y + 85, position.width, 20), unlocked, new GUIContent("unlocked"));

        EditorGUI.EndProperty();

        property.serializedObject.ApplyModifiedProperties();

    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label)*6 + 5; 
    }
}
