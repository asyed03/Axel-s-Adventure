using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cutscene : MonoBehaviour
{
    public int ID;
    public CharacterController2D player;
    public float waitStart = 0f;

    public bool startPos;
    public Vector3 startPoss;
    public CutsceneAction[] actions;

    private bool cutsceneStarted = false;
    private bool actiondone = false;

    // Start is called before the first frame update
    private void Start()
    {
        DialogueManager.instance.onCutsceneEnd += End;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !cutsceneStarted)
        {
            StartCoroutine(WalkToStart());
            TriggerCutscene();
        }
    }
    public void TriggerCutscene()
    {
        Debug.Log("triggered cutscene");
        cutsceneStarted = true;
        StartCoroutine(Timeline());
    }

    IEnumerator Timeline()
    {
        player.cutscene = true;
        yield return new WaitForSeconds(waitStart);

        foreach (CutsceneAction a in actions)
        {
            StartCoroutine(a.DoAction());
            if (a.WaitUntilEnd && a.events.GetPersistentTarget(0).GetType() == typeof(DialogueTrigger))
            {
                Debug.Log("wait until end");
                yield return new WaitUntil(() => actiondone);
                /*
                while (!actiondone)
                {
                    Debug.Log("waiting2");
                    DialogueTrigger dialogueTrigger = a.events.GetPersistentTarget(0) as DialogueTrigger;
                    if (!pressed && Input.GetKey(dialogueTrigger.key) && !DialogueManager.instance.isAnimating && DialogueManager.instance.canTrigger && !dialogueTrigger.dialogueActive)
                    {
                        pressed = true;
                        Debug.Log("triggered dialogue");
                        DialogueManager.instance.cutscene = true;
                        dialogueTrigger.dialogue.WriteNext(dialogueTrigger.speed, dialogueTrigger.animationType, dialogueTrigger.key);
                    }
                    yield return new WaitForEndOfFrame();
                }
                */
            }
            Debug.Log("done action");
            actiondone = false;
            yield return new WaitForSeconds(a.waitTime);
        }
        player.cutscene = false;
        player.frozen = false;
        yield return null;
    }

    IEnumerator WalkToStart()
    {
        player.rb.velocity = new Vector2(Mathf.Sign(startPoss.x - player.transform.position.x) * 3f, 0);
        yield return new WaitUntil(() => player.transform.position.x == startPoss.x);
        player.rb.velocity = Vector2.zero;
    }

    public void End()
    {
        actiondone = true;
        Debug.Log("done");
    }

    private void OnDisable()
    {
        DialogueManager.instance.onCutsceneEnd -= End;
    }
}
