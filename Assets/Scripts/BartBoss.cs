using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BartBoss : MonoBehaviour
{
    public float health;
    public float hitRadius;
    public float maxSpeed;

    public float lightDamage;
    public float knockbackPower; 

    public Rigidbody2D rb;

    public GameObject player;
    public LayerMask playerMask;
    public enum EnemyStates {idle, attacking, hurt, weak, dead, enraged}
    private EnemyStates enemyState;

    public Animator animator;

    void Start()
    {
        enemyState = EnemyStates.idle;
    }

    // Update is called once per frame
    void Update()
    {
        switch (enemyState)
        {
            case EnemyStates.idle:
                rb.velocity = Vector2.zero;
                animator.SetBool("idle", true);
                break;

            case EnemyStates.enraged:
                rb.velocity = new Vector2(Mathf.Sign(player.transform.position.x - transform.position.x) * maxSpeed * 2, 0);
                Attack("light", lightDamage*2, knockbackPower, 0.3f, 2f);
                break;

            case EnemyStates.attacking:
                rb.velocity = new Vector2(Mathf.Sign(player.transform.position.x - transform.position.x) * maxSpeed, 0);
                Debug.Log(rb.velocity); 
                Attack("light", lightDamage, knockbackPower, 0.3f, 2f);
                break;

            case EnemyStates.hurt:
                rb.velocity = Vector2.zero;
                TakeDamage(lightDamage);
                break;

            case EnemyStates.weak:
                rb.velocity = new Vector2(Mathf.PingPong(rb.velocity.x, maxSpeed/2), 0);
                Attack("light", lightDamage, knockbackPower, 0.3f, 2f);
                break;

            case EnemyStates.dead:
                rb.velocity = Vector2.zero;
                Attack("light", lightDamage, knockbackPower, 0.3f, 2f);
                break;

            default:
                break;
        }
    }

    public void StartBattle()
    {
        rb.isKinematic = false;
    }

    public void Attack(string attacktype, float damage, float knockback, float knocktime, float freezetime)
    {
        animator.SetTrigger(attacktype);

        Collider2D playerhit = Physics2D.OverlapCircle(transform.position, hitRadius, playerMask);

        if (playerhit != null)
        {
            playerhit.gameObject.GetComponent<CharacterController2D>().TakeDamage(damage, knockback, knocktime, freezetime, transform.position);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        animator.SetTrigger("hit");
    }

    public void SwitchState(string state)
    {
        Enum.TryParse<EnemyStates>(state, out EnemyStates st);
        enemyState = st;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hitRadius);
    }
}
