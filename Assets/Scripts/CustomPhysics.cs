using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPhysics : MonoBehaviour
{
    private Vector2 velocity;
    private Rigidbody2D rb;
    public float colliderRadius;
    public float friction;
    public ContactFilter2D cfilter;
    public RaycastHit2D[] results = new RaycastHit2D[16];
    // Start is called before the first frame update
    void Start()
    {
       if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    private void FixedUpdate()
    {
        velocity += Physics2D.gravity * Time.deltaTime;
        Vector2 deltapos = velocity * Time.deltaTime;
        Move(deltapos);
    }

    void Move(Vector2 deltapos)
    {
        RaycastHit2D[] hits = new RaycastHit2D[16];
        Debug.DrawRay(transform.position, deltapos, Color.green);
        if (deltapos.magnitude > 0.0001f)
        {
            int cast = rb.Cast(deltapos, cfilter, hits, deltapos.magnitude + colliderRadius);

            for (int i = 0; i < cast; i++)
            {
                Vector2 normal = results[i].normal;
                Debug.Log(normal);
                Debug.DrawRay(transform.position, normal, Color.green);
                Debug.DrawRay(transform.position, deltapos, Color.red);
                if (deltapos.x == 0 && normal.x == 0)
                {
                    deltapos = Vector2.zero;
                }
                else if (deltapos.x != 0 && normal.x == 0)
                {
                    deltapos.x -= friction;
                    deltapos.x = Mathf.Clamp(deltapos.x, 0, Mathf.Max(deltapos.x));
                    deltapos.y = 0;
                }
                else
                {
                    Debug.Log("happening");
                    Vector2 alongGround = new Vector2(-normal.y, normal.x);
                    float dot = Vector2.Dot(normal, alongGround);
                    deltapos = dot * alongGround;
                }
                float maxDist = hits[i].distance - colliderRadius;
                if (deltapos.magnitude >= maxDist)
                {
                    deltapos = deltapos.normalized * maxDist;
                }
            }
        } 
        transform.position += new Vector3(deltapos.x, deltapos.y, 0);
    }
}

