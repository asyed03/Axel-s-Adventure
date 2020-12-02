using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPhysics : MonoBehaviour
{
    private Vector2 velocity;
    private Vector2 Force;
    private Rigidbody2D rb;
    public float mass = 1;
    public float gravityMod = 1f;
    public Vector2 gravity = new Vector2(0, -9.8f);
    public Vector2 forceGravity;
    public ContactFilter2D cfilter;
    public RaycastHit2D[] results = new RaycastHit2D[16];
    public List<RaycastHit2D> hits = new List<RaycastHit2D>(16);
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cfilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        forceGravity = (gravity * gravityMod * mass);
        Force = forceGravity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        velocity += (Force/mass) * Time.deltaTime;
        Vector2 deltaPos = velocity * Time.deltaTime;
        rb.position += deltaPos;
        Force = CalculateForce(deltaPos);
    }


    public Vector2 CalculateForce(Vector2 deltaPos)
    {
        Vector2 newForce = Force;
        int count = rb.Cast(deltaPos, cfilter, results, deltaPos.magnitude);
        hits.Clear();
        for (int i = 0; i < count; i++)
        {
            hits.Add(results[i]);
        }

        for (int i = 0; i < hits.Count; i++)
        {
            Vector2 normal = hits[i].normal;
            Debug.Log(normal);
            Vector2 forceNormal = normal.normalized * forceGravity;
            Vector2 forceGravityAngle = forceGravity;
            if (Mathf.Abs(normal.x) > 0.01)
            {
                Vector2 moveAlongGround = new Vector2(-normal.y, normal.x);
                forceGravityAngle = (forceGravity * Mathf.Sin(Vector2.Angle(forceGravity, -forceNormal)));
            }
            newForce = forceNormal + forceGravityAngle;
        }

        Debug.Log(newForce);
        return newForce;
    }

}
