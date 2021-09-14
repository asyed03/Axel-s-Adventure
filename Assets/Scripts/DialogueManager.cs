using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class DialogueManager: MonoBehaviour
{
    public static DialogueManager instance;
    public GameObject DialogueBox;
    public bool isAnimating = false;
    public bool canTrigger = true;
    public bool cutscene = false;
    public float currentSpeed = 0.1f;
    public KeyCode currentKey = KeyCode.E;
    public string currentAnim = "none";

    private GameObject dialogueObject = null;

    public delegate void CutsceneEnd();
    public event CutsceneEnd onCutsceneEnd;

    void Awake()
    {
        MakeSingleton();
    }

    void MakeSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void WriteSentence(Dialogue d, float typeSpeed, string animType, KeyCode key)
    {
        currentSpeed = typeSpeed;
        currentAnim = animType;
        currentKey = key;
        if (d.index >= d.sentences.Length)
        {
            EndDialogue(d);
            return;
        }
        if (d.index == 0)
        {
            dialogueObject = GameObject.FindGameObjectWithTag("Dialog");
            if (dialogueObject == null)
            {
                dialogueObject = Instantiate(DialogueBox, GameObject.Find("LevelUI").transform);
            }    
            foreach (Transform child in dialogueObject.transform)
            {
                if (child.gameObject.name == "Character")
                {
                    child.gameObject.GetComponent<Image>().sprite = d.displaySprite;
                    child.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = d.name;
                }
                if (child.gameObject.name == "Text")
                {
                    d.TextElement = child.gameObject.GetComponent<TextMeshProUGUI>();
                }
            }
            d.animator = dialogueObject.GetComponent<Animator>();
            d.animator.SetBool("slidein", true);
        }
        switch (animType)
        {
            default:
                d.TextElement.text = d.sentences[d.index];
                break;
                
            case "TypeWriter":
                StartCoroutine(Typewriter(d, d.sentences[d.index], typeSpeed, d.TextElement, key));
                break;
        }
    }

    IEnumerator Typewriter(Dialogue d, string sentence, float speed, TextMeshProUGUI text, KeyCode key)
    {
        Debug.Log("typing");
        isAnimating = true;
        text.text = "";
        float speedMultiplier;
        foreach (char ch in sentence.ToCharArray())
        {
            if (Input.GetKey(key))
            {
                speedMultiplier = speed/2;
            }
            else
            {
                speedMultiplier = speed;
            }
            yield return new WaitForSeconds(speedMultiplier * Time.deltaTime);
            text.text += ch;
        }
        isAnimating = false;

        yield return new WaitUntil(() => Input.GetKeyDown(key));

        d.index += 1;
        WriteSentence(d, speed, "TypeWriter", key);
    }
   

    public void EndDialogue(Dialogue d)
    {
        if (cutscene)
        {
            if (onCutsceneEnd != null)
            {
                Debug.Log("dialogue done event fired");
                onCutsceneEnd();
            }
            cutscene = false;
            d.index = 0;
        }
    }

    public void slideout()
    {
        if (dialogueObject != null)
        {
            dialogueObject.GetComponent<Animator>().SetBool("slidein", false);
            StartCoroutine(DestroyAfterSlide(dialogueObject.GetComponent<DialogueTrigger>()));
        }
    }

    public IEnumerator DestroyAfterSlide(DialogueTrigger dialogueTrigger)
    {
        canTrigger = false;
        //dialogueTrigger.dialogue.index = 0; 
        yield return new WaitForSeconds(0.5f);
        Destroy(dialogueObject);
        canTrigger = true;
    }
}
