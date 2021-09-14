using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CutsceneAction
{
    public GameObject target;

    public float waitTime;

    public bool WaitUntilEnd;

    public UnityEvent events;

    public IEnumerator DoAction()
    {
        events.Invoke(); 
        yield return null;
    }


}