using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PressableButton : MonoBehaviour
{
    private float triggerMass = 1f;
    public LayerMask interactables;
    public Animator anim;
    public bool isPressed = false;
    public float currentWeight = 0f;
    public GameObject[] effectedItems;
    [HideInInspector]
    public int selected = 0;
    [HideInInspector]
    public int selected2 = 0;
    public List<MethodInfo> onButtonDown = new List<MethodInfo>();
    public List<MethodInfo> onButtonUp = new List<MethodInfo>();

    public string className;

    void OnEnable()
    {
        var mbs = effectedItems[0].GetComponent<MonoBehaviour>().GetType().GetMethods();
        onButtonDown.AddRange(mbs);
        onButtonUp.AddRange(mbs);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collision registered");

        if (hasLayerMask(interactables, collision.gameObject.layer))
        {
            currentWeight += collision.attachedRigidbody.mass;
        }

        if (currentWeight >= triggerMass)
        {
            isPressed = true;
            anim.SetBool("isPressed", isPressed);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("collision exit registered");

        if (hasLayerMask(interactables, collision.gameObject.layer))
        {
            currentWeight -= collision.attachedRigidbody.mass;
        }

        if (currentWeight < triggerMass)
        {
            isPressed = false;
            anim.SetBool("isPressed", isPressed);
        }
    }

    public void ButtonPress()
    {
        if (isPressed)
        {
            for (int i = 0; i < effectedItems.Length; i++)
            {
                onButtonDown[selected].Invoke(effectedItems[i].GetComponent(className), null);
            }
        }
        else
        {
            for (int i = 0; i < effectedItems.Length; i++)
            {
                onButtonUp[selected2].Invoke(effectedItems[i].GetComponent(className), null);
            }
        }
    }

    private bool hasLayerMask(LayerMask layerMask, int layer)
    {
        if (layerMask == (layerMask | (1 << layer)))
        {
            return true;
        }

        return false;
    }
}
