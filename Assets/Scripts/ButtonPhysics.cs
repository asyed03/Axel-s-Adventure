using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPhysics : MonoBehaviour
{
    public CharacterController2D player;
    private Rigidbody2D rb;
    private BoxCollider2D collider2d;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //push box on top of button
        if (collision.gameObject.CompareTag("Button") && collider2d.IsTouching(player.boxcollider) && player.anim.GetCurrentAnimatorStateInfo(0).IsName("push"))
        {
            float boxBottom = collider2d.bounds.center.y - collider2d.bounds.extents.y;
            float buttonTop = collision.gameObject.GetComponent<Collider2D>().bounds.center.y + collision.gameObject.GetComponent<Collider2D>().bounds.extents.y;
            if (boxBottom <= buttonTop)
            {
                rb.velocity = Vector2.up;
            }        
        }
    }
}
