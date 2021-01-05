using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Camera cam;
    public Vector2 parallaxEffect;
    private Vector3 prevPos;
    private float textureUnitSizeX;
    private float textureUnitSizeY;
    public bool lockPos;
    public bool infiniteX;
    public bool infiniteY;
    private Vector2 diff;
    //Start is called before the first frame update
    void Start()    
    {
        cam = Camera.main;
        prevPos = cam.transform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = (texture.width / sprite.pixelsPerUnit);
        textureUnitSizeY = (texture.height / sprite.pixelsPerUnit);
        diff = transform.position - cam.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector2 camSpeed = cam.transform.position - prevPos;
        if (lockPos)
        {
            transform.position += new Vector3(camSpeed.x * parallaxEffect.x, cam.transform.position.y - transform.position.y, 0);
        }
        else
        {
            transform.position += new Vector3(camSpeed.x * parallaxEffect.x, camSpeed.y * parallaxEffect.y, 0);
        }
        prevPos = cam.transform.position;
        if (infiniteX)
        {
            if (Mathf.Abs(cam.transform.position.x - transform.position.x) >= textureUnitSizeX)
            {
                float offsetPosX = (cam.transform.position.x - transform.position.x) % textureUnitSizeX;
                transform.position = new Vector3(cam.transform.position.x + offsetPosX, transform.position.y);
            }
        }
        
        if (infiniteY)
        {
            if (Mathf.Abs(cam.transform.position.y - transform.position.y) >= textureUnitSizeY)
            {
                float offsetPosY = (cam.transform.position.y - transform.position.y) % textureUnitSizeY;
                transform.position = new Vector3(transform.position.x, cam.transform.position.y + offsetPosY);
            }
        }
    }
}
