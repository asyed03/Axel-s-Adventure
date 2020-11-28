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
    public BoxCollider2D col;
    public Animator anim;
    public bool isPressed = false;
    public float currentWeight = 0f;
    public GameObject effected;
    [HideInInspector]
    public int selected = 0;
    public List<MethodInfo> methods = new List<MethodInfo>();
    public string className;

    void OnEnable()
    {
        var mbs = effected.GetComponent<MonoBehaviour>().GetType().GetMethods();
        methods.AddRange(mbs);
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
            methods[selected].Invoke(effected.GetComponent(Type.GetType(className)), null);
        }
        else
        {
            methods[selected].Invoke(effected.GetComponent(Type.GetType(className)), null);
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
