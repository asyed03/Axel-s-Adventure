using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AfterImage : MonoBehaviour
{
    public GameObject afterImage;
    public Animator anim;
    public float imageDelay;
    public float destroyDelay;
    public bool makeImage = false;

    private SpriteRenderer spriteRenderer;
    private SpriteRenderer spRenderer;
    private float imageDelayTimer = 0f;
    private float alphaset;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (makeImage)
        {
            if (imageDelayTimer > 0)
            {
                imageDelayTimer -= Time.deltaTime;
            }
            else
            {
                GameObject afterImageObject = Instantiate(afterImage, transform.position, transform.rotation);
                anim = afterImageObject.GetComponent<Animator>();
                spRenderer = afterImageObject.GetComponent<SpriteRenderer>();
                spRenderer.sprite = spriteRenderer.sprite;
                anim.Play("AfterImage");
                imageDelayTimer = imageDelay;
                Destroy(afterImageObject, destroyDelay);
            }
        }
    }
}
