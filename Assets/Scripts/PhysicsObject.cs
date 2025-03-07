﻿using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UIElements;

public class PhysicsObject : MonoBehaviour
{

    protected Vector2 velocity;
    protected Vector2 gravity;
    protected Vector2 slopeVelocity;
    protected float speed = 0.05f;
    protected float gravityMod = 1f;
    protected const float minMoveDistance = 0.001f;
    protected const float shellradius = 0.05f;
    protected Vector2 groundNormal;

    public Rigidbody2D rb;
    public bool grounded = false;
    public float minGroundNormalY = 0.65f;
    protected RaycastHit2D[] hitpoints = new RaycastHit2D[16];
    protected List<RaycastHit2D> listhitpoints = new List<RaycastHit2D>(16);
    protected ContactFilter2D cfilter;
    protected Vector2 direction;


    // Start is called before the first frame update
    void Start()
    {
        cfilter.useTriggers = false;
        cfilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
    }

    void FixedUpdate()
    {
        /*
        velocity += gravityMod * Physics2D.gravity * Time.deltaTime;
        grounded = false;
        Vector2 deltapos = velocity * Time.deltaTime;
        Move(deltapos, true);
        */
        gravity += gravityMod * Physics2D.gravity * Time.deltaTime;
        velocity += gravityMod * Physics2D.gravity * Time.deltaTime;
        Vector2 deltapos = velocity * Time.deltaTime;

        Movement(deltapos);
    }

    // Update is called once per frame
    void Update()
    {
        //rb.position += speed * Physics2D.gravity * Time.deltaTime;
        //print(rb.velocity);
    }

    protected void Move(Vector2 move, bool ymovement)
    {
        float distance = move.magnitude;

        if (distance > minMoveDistance)
        {
            int count = rb.Cast(move, cfilter, hitpoints, distance + shellradius);
            listhitpoints.Clear();
            for (int i = 0; i < count; i++)
            {
                listhitpoints.Add(hitpoints[i]);
            }

            for (int i = 0; i < listhitpoints.Count; i++)
            {
                Vector2 currentNormal = listhitpoints[i].normal;

                Debug.DrawRay(transform.position, currentNormal, Color.green, 1);
                if (currentNormal.y > minGroundNormalY)
                {
                    grounded = true;
                    if (ymovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(currentNormal, velocity);

                if (projection > 0)
                {
                    velocity = velocity - projection * currentNormal;
                }

                float modifiedDistance = listhitpoints[i].distance - shellradius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;

            }
        }
        rb.position += move.normalized * distance;
    }

    protected void Movement(Vector2 move)
    {
        float distance = move.magnitude;

        int raycast = rb.Cast(move, cfilter, hitpoints, shellradius + distance);

        listhitpoints.Clear();
        for (int i = 0; i < raycast; i++)
        {
            listhitpoints.Add(hitpoints[i]);
        }

        for (int i = 0; i < listhitpoints.Count; i++)
        {
            groundNormal = listhitpoints[i].normal;

            if (groundNormal.y > minGroundNormalY)
            {
                grounded = true;
            }

            Vector2 moveAlongGround = new Vector2(-groundNormal.y, groundNormal.x);
            float projection = Vector2.Dot(groundNormal, velocity);
            float projection2 = Vector2.Dot(gravity, moveAlongGround);

            if (groundNormal.x != 0)
            {
                Debug.DrawRay(rb.transform.position, moveAlongGround);
                Debug.Log("True2");
                velocity += projection2 * moveAlongGround;
            }

            float mindist = listhitpoints[i].distance - shellradius;

            distance = mindist < distance ? mindist : distance;
        }

        Debug.DrawRay(rb.transform.position, velocity, Color.green);
        rb.position += move.normalized * distance;
    }
}