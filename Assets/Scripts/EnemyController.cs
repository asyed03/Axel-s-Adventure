using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    public float health = 5f;
    public Rigidbody2D rb;
    public Rigidbody2D playerRb;
    public SpriteRenderer spriteRenderer;
    public CharacterController2D playerController;
    public Collider2D col;
    public float damage;

    public float speed;
    public float playerKnockback;
    public float playerKnockbackTime;
    [Range(0, 5)]
    public float moveRange;
    public float knockBackTime;
    public float freezeTime;

    private float knockBackTimer = 0f;

    private Vector2 startPos;
    private bool facingRight = true;
    private bool isHit = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(transform.position, new Vector2(-playerRb.velocity.normalized.x, 1) * playerKnockback);
        Debug.DrawRay(transform.position, new Vector2(transform.right.normalized.x, 1) * 5);
        knockBackTimer -= Time.deltaTime;

        if (col.IsTouchingLayers(LayerMask.GetMask("Ground")) && knockBackTimer <= 0)
        {
            if (startPos == null)
            {
                startPos = transform.position;
            }

            rb.velocity = transform.right * speed;

            if (transform.position.x > startPos.x + moveRange && facingRight || transform.position.x < startPos.x - moveRange && !facingRight)
            {
                facingRight = !facingRight;
                transform.Rotate(new Vector3(0, 180, 0));
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (LayerMask.LayerToName(collision.collider.gameObject.layer) == "Player")
        {
            rb.velocity = Vector2.zero;
            //Debug.Log("Player: " + collision.collider.bounds.min.y + " Enemy: " + (col.bounds.max.y));
            if (collision.collider.bounds.min.y >= col.bounds.max.y-0.03)
            {
                playerRb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                TakeDamage(playerController.attackDamage, freezeTime, 1f);
            }
            else
            {
                playerRb.velocity = Vector2.zero;
                playerController.TakeDamage(damage, playerKnockback, playerKnockbackTime, transform.position);
                knockBackTimer = freezeTime;
            }
        }
    }

    public void TakeDamage(float damage, float knockTime, float knockBackPower)
    {
        AudioManager.instance.Play("bump_sound", "Once");
        health -= damage;

        if (health <= 0)
        {
            Die(true);
        }
        else
        {
            StartCoroutine(AnimateHit());

            knockBackTimer = knockTime;

            Vector2 dir = rb.transform.position - playerRb.transform.position;
            rb.velocity = new Vector2(dir.normalized.x, 1) * knockBackPower;
        }
    }

    void Die(bool animation)
    {
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        rb.gravityScale = 0;
        col.enabled = false;
        if (animation)
        {
            StartCoroutine(AnimateDeath());
        }
    }

    private IEnumerator AnimateDeath()
    {
        spriteRenderer.enabled = !spriteRenderer.enabled;
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.enabled = !spriteRenderer.enabled;
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.enabled = !spriteRenderer.enabled;
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.enabled = !spriteRenderer.enabled;
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.enabled = !spriteRenderer.enabled;
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.enabled = !spriteRenderer.enabled;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    private IEnumerator AnimateHit()
    {
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.color = new Color(250, 0, 0, 255);
    }
}
