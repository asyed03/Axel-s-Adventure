using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Enter");
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && collision.gameObject.GetComponent<CharacterController2D>().canClimb == true)
        {
            collision.GetComponent<CharacterController2D>().transform.position = new Vector2(transform.position.x, collision.GetComponent<CharacterController2D>().transform.position.y);
            collision.GetComponent<CharacterController2D>().Climb();
            collision.GetComponent<CharacterController2D>().ropeCollider = gameObject.GetComponent<Collider2D>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Exit");
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && collision.gameObject.GetComponent<CharacterController2D>().canClimb == true)
        {
            collision.GetComponent<CharacterController2D>().Climb();
        }
    }
}
