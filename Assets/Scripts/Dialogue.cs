using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public Sprite displaySprite;
    public string name;

    [TextArea(3, 15)]
    public string[] sentences;
    public int index = 0;
    [HideInInspector]
    public TextMeshProUGUI TextElement;
    public Animator animator;
    public Dialogue nextDialogue;

    public void WriteNext(float speed, string anim, KeyCode key)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>().frozen = true;
        DialogueManager.instance.WriteSentence(this, speed, anim, key);
    }
}
