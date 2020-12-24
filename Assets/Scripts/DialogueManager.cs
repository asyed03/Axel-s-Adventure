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
            if (!isAnimating)
            {
                EndDialogue(d);
            } 
            return;
        }
        else if (isAnimating)
        {
            return;
        }
        if (d.index == 0)
        {
            dialogueObject = GameObject.Find("Dialogue Box(Clone)");
            if (dialogueObject == null)
            {
                dialogueObject = Instantiate(DialogueBox, GameObject.Find("LevelUI").transform);
            }    
            foreach (Transform child in dialogueObject.transform)
            {
                if (child.gameObject.name == "Name")
                {
                    child.gameObject.GetComponent<TextMeshProUGUI>().text = d.name;
                }
                if (child.gameObject.name == "Character")
                {
                    child.gameObject.GetComponent<Image>().sprite = d.displaySprite;
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
                StartCoroutine(Typewriter(d.sentences[d.index], typeSpeed, d.TextElement, key));
                d.index += 1;
                break;
        }
        
    }

    IEnumerator Typewriter(string sentence, float speed, TextMeshProUGUI text, KeyCode key)
    {
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
            yield return new WaitForSeconds(speedMultiplier);
            text.text += ch;
        }
        isAnimating = false;
    }

    public void EndDialogue(Dialogue d)
    {
        if (d.nextDialogue != null)
        {
            d.nextDialogue.WriteNext(currentSpeed, currentAnim, currentKey);
        }
        else
        {
            d.animator.SetBool("slidein", false);
            if (dialogueObject != null) 
            {
                StartCoroutine(DestroyAfterSlide(d));
            } 
        }   
    }

    IEnumerator DestroyAfterSlide(Dialogue dialogue)
    {
        canTrigger = false;
        yield return new WaitForSeconds(0.5f);
        dialogue.index = 0;
        if (cutscene)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>().frozen = false;
            if (onCutsceneEnd != null)
            {
                onCutsceneEnd();
            }        
            cutscene = false;
        }
        Destroy(dialogueObject);
        canTrigger = true;
    }
}
