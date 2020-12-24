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
    public enum EnemyType {square, slime};
    public EnemyType enemyType;
    public float health = 5f;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public Collider2D col;
    public float damage;
    public bool isDead = false;

    public float speed;
    public float playerKnockback;
    public float playerKnockbackTime;
    public float playerFreezeTime;
    [Range(0, 5)]
    public float moveRange;
    public float freezeTime;
    public bool frozen = false;

    private float knockBackTimer = 0f;

    private Vector2 startPos;
    private bool facingRight = true;
    //private bool isHit = false;

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(transform.position, new Vector2(-playerRb.velocity.normalized.x, 1) * playerKnockback);
        knockBackTimer -= Time.deltaTime;

        if (col.IsTouchingLayers(LayerMask.GetMask("Ground")) && knockBackTimer <= 0 && !isDead && !frozen)
        {
            if (startPos == null)
            {
                startPos = transform.position;
            }

            if (enemyType == EnemyType.slime)
            {
                GetComponent<Animator>().SetFloat("xvel", Mathf.Abs(rb.velocity.x));
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
        if (LayerMask.LayerToName(collision.collider.gameObject.layer) == "Player" && knockBackTimer <= 0)
        {
            rb.velocity = Vector2.zero;
            collision.gameObject.GetComponent<CharacterController2D>().TakeDamage(damage, playerKnockback, playerKnockbackTime, playerFreezeTime, transform.position);
            knockBackTimer = freezeTime;
        }
    }

    public void TakeDamage(float damage, float knockTime, float knockBackPower, Vector3 playerPos)
    {   
        AudioManager.instance.Play("bump_sound", "Once");
        health -= damage;

        if (health <= 0)
        {
            isDead = true;
            if (enemyType == EnemyType.slime)
            {
                GetComponent<Animator>().SetTrigger("isHit");
                GetComponent<Animator>().SetTrigger("isDead");
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
                col.enabled = false;
            }
            else
            {
                Die();
            }
        }
        else
        {
            if (enemyType == EnemyType.slime)
            {
                GetComponent<Animator>().SetTrigger("isHit");
            }
            else
            {
                StartCoroutine(AnimateHit());
            }

            knockBackTimer = knockTime;

            Vector2 dir = rb.transform.position - playerPos;
            rb.velocity = new Vector2(dir.normalized.x, 1) * knockBackPower;
        }
        Debug.DrawRay(transform.position, rb.velocity);
    }

    void Die()
    {
        StartCoroutine(AnimateDeath());
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
        var temp = spriteRenderer.color;
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.color = temp;
    }
}
