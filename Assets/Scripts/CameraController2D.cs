using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController2D : MonoBehaviour
{
    public RectTransform focusBounds;
    public RectTransform worldBounds;
    public GameObject character;
    public float followSpeed = 1f;
    public float shakeRange = 0.5f;
    public float shakeTimer;
    public Camera cam;
    private Vector3[] focusCorners = new Vector3[4];
    private Vector3[] worldCorners = new Vector3[4];
    private Vector3 desiredPos;
    public bool follow = false;
    // Start is called before the first frame update
    void Start()
    {
        if (cam == null)
        {
            cam = GetComponent<Camera>();
        }
        //transform.position = new Vector3(character.transform.position.x, character.transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        focusBounds.GetWorldCorners(focusCorners);
        worldBounds.GetWorldCorners(worldCorners);
        float vertExtent = cam.orthographicSize;
        float horzExtent = vertExtent * cam.aspect;
        desiredPos = transform.position;

        if (shakeTimer <=0)
        {
            CancelInvoke("SetRandomPos");
            shakeTimer = 0;
        }
        else
        {
            shakeTimer -= Time.deltaTime;
        }
        if (follow && cam.WorldToScreenPoint(character.transform.position).x > cam.WorldToScreenPoint(focusCorners[2]).x ||
            cam.WorldToScreenPoint(character.transform.position).x < cam.WorldToScreenPoint(focusCorners[0]).x ||
            cam.WorldToScreenPoint(character.transform.position).y > cam.WorldToScreenPoint(focusCorners[2]).y ||
            cam.WorldToScreenPoint(character.transform.position).y < cam.WorldToScreenPoint(focusCorners[0]).y)
        {
            float offsetX = transform.position.x - focusBounds.position.x;
            float offsetY = transform.position.y - focusBounds.position.y;

            desiredPos = new Vector3(character.transform.position.x + offsetX, character.transform.position.y + offsetY, transform.position.z);          
        }
        transform.position = Vector3.Lerp(transform.position, desiredPos, followSpeed);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, worldCorners[0].x + horzExtent, worldCorners[2].x - horzExtent),
                                         Mathf.Clamp(transform.position.y, worldCorners[0].y + vertExtent, worldCorners[2].y - vertExtent),
                                         transform.position.z);
    }

    public void Shake(float duration, float frequency)
    {
        shakeTimer = duration;
        InvokeRepeating("SetRandomPos", 0, frequency);
    }

    public void SetRandomPos()
    {
        Vector2 randomPos = Random.insideUnitCircle * shakeRange;
        transform.position = new Vector3(transform.position.x + randomPos.x, transform.position.y + randomPos.y, transform.position.z);
    }
}
