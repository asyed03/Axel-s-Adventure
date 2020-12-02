using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public float startTime = 0f;
    public float speed = 1f;
    public float damage = 100f;
    public bool on = true;
    public Animator anim;

    private void Start()
    {
        anim.SetBool("On", on);
        anim.speed = 0;
        StartCoroutine(Wait(startTime));
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (on && LayerMask.LayerToName(collision.collider.gameObject.layer) == "Player")
        {
            collision.collider.gameObject.GetComponent<CharacterController2D>().TakeDamage(damage, 0, 2, 2, transform.position);
        }
    }

    public void ToggleState()
    {
        on = !on;
        anim.SetBool("On", on);
    }

    public void ChangeSpeed()
    {
        anim.speed = speed;
    }
    private IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        anim.speed = speed;
    }
}
