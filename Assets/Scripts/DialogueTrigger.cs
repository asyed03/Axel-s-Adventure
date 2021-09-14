using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public bool cutscene = false;
    public bool dialogueActive = false;
    public string cutsceneName;
    public string animationType;
    public float speed = 0.1f;
    public KeyCode key;

    private PlayableDirector timeline;
    private void Start()
    {
        /*
        if (cutscene)
        {
            DialogueManager.instance.onCutsceneEnd += EndCutscene;
            timeline = GameObject.Find(cutsceneName).GetComponent<PlayableDirector>();
            /*
            timeline.playableGraph.GetRootPlayable(0).SetSpeed(0);
            DialogueManager.instance.cutscene = true;
            dialogue.WriteNext(0.1f, "TypeWriter", key);
        }
        */
    }

    // Update is called once per frame
    public void Update()
    {
        if (!cutscene && Input.GetKeyDown(key) && DialogueManager.instance.canTrigger && !DialogueManager.instance.isAnimating)
        {
            Debug.Log("triggered dialogue");
            dialogue.WriteNext(speed, animationType, key);
        }
    }

    public void WriteNext()
    {
        dialogue.WriteNext(speed, animationType, key);
    }

    public void WriteNextCutscene() 
    {
        DialogueManager.instance.cutscene = true;
        dialogue.WriteNext(speed, animationType, key);
        /*
        Debug.Log("triggered cutscene dialogue");
        timeline = GameObject.Find(cutsceneName).GetComponent<PlayableDirector>();
        timeline.playableGraph.GetRootPlayable(0).SetSpeed(0);
        dialogueActive = true;
        DialogueManager.instance.cutscene = true;
        dialogue.WriteNext(speed, animationType, key);
        */
    }

    public void EndCutscene()
    {
        Debug.Log("ended dialogue");
        timeline.playableGraph.GetRootPlayable(0).SetSpeed(1);
        dialogueActive = false;
    }

    private void OnDisable()
    {
        DialogueManager.instance.onCutsceneEnd -= EndCutscene;
    }
}
