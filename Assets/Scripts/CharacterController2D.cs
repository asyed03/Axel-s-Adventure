using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    public float jump = 10f;
    public float speed = 2f;
    public float climbSpeed = 0.5f;
    public float maxSpeed = 5f;
    public float dashSpeed = 5f;
    public float dashTime = 2f;
    public float fallSpeed = 2f;
    public float jumpFallSpeed = 1.5f;
    public Transform attackPosition;
    public float attackDamage = 5f;
    public float attackRange = 1f;
    public float attackFreezeTime = 0.5f;
    public float attackKnockPower = 3f;
    public float attackTime = 1f;
    public bool cutscene = false;
    public bool frozen = false;
    public bool isFacingRight = true;
    public bool grounded = false;
    public bool isCrouching = false;
    public bool isDashing = false;
    public bool specialPressed = false;
    public bool isHit = false;
    public bool isDead = false;
    public bool isClimbing = false;
    public bool canClimb = true;
    public bool isTouchingWall = false;
    public CameraController2D CameraController;
    public Rigidbody2D rb;
    public BoxCollider2D boxcollider;
    public SpriteRenderer spriteRenderer;
    public LayerMask groundLayers;
    public LayerMask wallLayers;
    public Animator anim;
    public ParticleSystem dust;

    public float FreezeTimer = 0f;
    public float KnockTimer = 0f;
    public float DashTimer = 0f;
    public float AttackTimer = 0f;
    private Vector2 dashDir;
    private AfterImage AfterImageScript;
    [HideInInspector]
    public Collider2D ropeCollider;


    // Start is called before the first frame update
    void Start()
    {
        AfterImageScript = GetComponent<AfterImage>();
        GameManager.instance.health = 100;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("xvelocity", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("yvelocity", rb.velocity.y);
        anim.SetBool("isGrounded", grounded);
        anim.SetBool("isCrouching", isCrouching);
        anim.SetBool("isClimbing", isClimbing);
        anim.SetBool("isClimbingMovement", false);
        anim.SetBool("isHit", isHit);
        anim.SetBool("isTouchingWall", isTouchingWall);

        FreezeTimer -= Time.deltaTime;
        if (FreezeTimer > 0)
        {
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), true);
        }
        else
        {
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), false);
            spriteRenderer.color = Color.white;
        }
        if (KnockTimer > 0)
        {
            KnockTimer -= Time.deltaTime;
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        DashTimer -= Time.deltaTime;
        AttackTimer -= Time.deltaTime;

        //Dash movement update
        if (isDashing)
        {
            if (isClimbing || DashTimer <= 0)
            {
                DashTimer = 0;
                isDashing = false;
                AfterImageScript.makeImage = false;
                rb.velocity = Vector2.zero;
            }
            else
            {
                rb.velocity = dashDir * dashSpeed;
            }
        }

        if (isClimbing)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            rb.transform.position += new Vector3(Input.GetAxisRaw("Horizontal") * 0.5f, Input.GetAxisRaw("Vertical"), 0) * climbSpeed * Time.deltaTime;

            if (transform.position.y > ropeCollider.bounds.max.y-0.01f)
            {
                transform.position  = new Vector2(transform.position.x, ropeCollider.bounds.max.y - 0.01f);
            }
            
            anim.SetBool("isClimbingMovement", (Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0));

            if (Input.GetAxis("Horizontal") > 0 && !isFacingRight)
            {
                Flip();
            }
            else if (Input.GetAxis("Horizontal") < 0 && isFacingRight)
            {
                Flip();
            }
        }

        if (FreezeTimer <= 0 && GameManager.instance.GamePaused == false && !isDead && !isClimbing && !frozen && !cutscene)
        {
            isHit = false;

            Debug.DrawRay(transform.position, (Vector2.up * Input.GetAxis("Vertical") + Vector2.right * Input.GetAxis("Horizontal")).normalized * dashSpeed, Color.red);
            
            //Dash Check
            if (Input.GetKeyDown(KeyCode.LeftShift) && !grounded && !isDashing && !specialPressed)
            {
                AfterImageScript.makeImage = true;
                Dash(true, 0.2f, 0.05f);
                FreezeTimer = dashTime;
            }
            //

            //Crouch Check     
            isCrouching = Input.GetKey(KeyCode.S);
            //

            if (!isCrouching) 
            {
                //Attack Check
                if (Input.GetKeyDown(KeyCode.F) && grounded && AttackTimer <= 0f)
                {
                    rb.velocity = Vector2.zero;
                    FreezeTimer = attackFreezeTime;
                    AttackTimer = attackTime;
                    Attack();               
                }
                //

                //Jump Check
                if (Input.GetKeyDown(KeyCode.Space) && grounded)
                {
                    //vertical movement
                    AudioManager.instance.Play("jump_sound", "Once");
                    rb.velocity += Vector2.up * jump;
                }
                //
            }

            //horizontal movement
            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, rb.velocity.y);
        }
    }

    void FixedUpdate()
    {
        GroundCheck();
        //better jump
        Move();
    }

    private void Move()
    {
        if (rb.velocity.y < 0 || FreezeTimer >= 0)
        {
            rb.velocity += Physics2D.gravity * fallSpeed * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.velocity += Physics2D.gravity * jumpFallSpeed * Time.deltaTime;
        }

        //Flip
        if ((rb.velocity.x < -0.01f && isFacingRight) || (rb.velocity.x > 0.01f && !isFacingRight))
        {
            Flip();
        }

        //speed limit
        //Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed);
    }

    private void Flip()
    {
        CreateDust();
        transform.Rotate(0, 180, 0);
        isFacingRight = !isFacingRight;
    }
    private void GroundCheck() 
    {
        RaycastHit2D hit = Physics2D.Raycast(boxcollider.bounds.center, Vector2.down, boxcollider.bounds.extents.y + 0.05f, groundLayers);
        RaycastHit2D hitL = Physics2D.Raycast(boxcollider.bounds.center - (Vector3.right * boxcollider.size.x/ 2.5f), Vector2.down, boxcollider.bounds.extents.y + 0.05f, groundLayers);
        RaycastHit2D hitR = Physics2D.Raycast(boxcollider.bounds.center + (Vector3.right * boxcollider.size.x / 2.5f), Vector2.down, boxcollider.bounds.extents.y + 0.05f, groundLayers);
        RaycastHit2D hitRW = Physics2D.Raycast(boxcollider.bounds.center, Vector2.right, boxcollider.bounds.extents.x + 0.01f, wallLayers);
        RaycastHit2D hitLW = Physics2D.Raycast(boxcollider.bounds.center, Vector2.left, boxcollider.bounds.extents.x + 0.01f, wallLayers);

        Debug.DrawRay(boxcollider.bounds.center, Vector2.down * (boxcollider.bounds.extents.y + 0.05f), Color.green);
        Debug.DrawRay(boxcollider.bounds.center - (Vector3.right * boxcollider.size.x / 2.5f), Vector2.down * (boxcollider.bounds.extents.y + 0.05f), Color.green);
        Debug.DrawRay(boxcollider.bounds.center + (Vector3.right * boxcollider.size.x / 2.5f), Vector2.down * (boxcollider.bounds.extents.y + 0.05f), Color.green);
        Debug.DrawRay(boxcollider.bounds.center, Vector2.right * (boxcollider.bounds.extents.x + 0.01f), Color.red);
        Debug.DrawRay(boxcollider.bounds.center, Vector2.left * (boxcollider.bounds.extents.x + 0.01f), Color.red);

        if (hit.collider != null || hitL.collider != null || hitR.collider != null)
        {
            isTouchingWall = (hitRW.collider != null || hitLW.collider != null) && ((isFacingRight && Input.GetKey(KeyCode.D)) || (!isFacingRight && Input.GetKey(KeyCode.A)));
            if (grounded == false)
            {
                CreateDust();
            }
            grounded = true;
            DashTimer = 0;
            canClimb = true;
            specialPressed = false;
        }
        else
        {
            grounded = false;
            if (isTouchingWall)
                isTouchingWall = false;
        }
    }

    private void Attack()
    {
        anim.SetTrigger("Attack");
        StartCoroutine(Waitforanim());
    }

    private IEnumerator Waitforanim()
    {
        yield return new WaitForSeconds(0.1f);
        var results = Physics2D.OverlapCircleAll(attackPosition.position, attackRange);
        foreach (Collider2D c in results)
        {
            if (c.CompareTag("Enemy"))
            {
                c.GetComponent<EnemyController>().TakeDamage(attackDamage, 1, attackKnockPower, transform.position);
            }
        }
        rb.velocity = Vector2.zero; 
    }
    public void Dash(bool cameraShake, float duration, float frequency)
    {
        CreateDust();
        float HorizontalAxis = Input.GetAxis("Horizontal");
        float VerticalAxis = Input.GetAxis("Vertical");
        //vertical movement

        DashTimer = dashTime;
        if ((Mathf.Abs(HorizontalAxis) > 0 || Mathf.Abs(VerticalAxis) > 0) && !isClimbing)
        {
            dashDir = ((Vector2.up * VerticalAxis) + (Vector2.right * HorizontalAxis)).normalized * dashSpeed;
        }
        else
        {
            dashDir = transform.right * dashSpeed;
        }

        rb.velocity = Vector2.zero;
        isDashing = true;
        specialPressed = true;
        DashTimer = dashTime;
        isClimbing = false;

        if (cameraShake)
        {
            CameraController.Shake(duration, frequency);
        }
        /*
        if (Mathf.Abs(HorizontalAxis) > 0.1 && Mathf.Abs(VerticalAxis) > 0.1)
        {
            Vector2 combinedDir = Vector2.up * VerticalAxis + Vector2.right * HorizontalAxis;
            rb.velocity = combinedDir.normalized * dashSpeed;
        }
        else if (Mathf.Abs(HorizontalAxis) > 0.1 && Mathf.Abs(VerticalAxis) < 0.1)
        {
            rb.velocity = (Vector2.right * HorizontalAxis).normalized * dashSpeed;
        }
        else if (Mathf.Abs(HorizontalAxis) < 0.1 && Mathf.Abs(VerticalAxis) > 0.1)
        {
            rb.velocity = (Vector2.up * VerticalAxis).normalized * dashSpeed;
        }
        else
        {
            rb.velocity = transform.right * dashSpeed;
        }
        */
    }

    public void Climb()
    {
        isClimbing = !isClimbing;
        rb.velocity = Vector2.zero;
        if (isClimbing)
        {
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = 1;
        }
    }
    public void TakeDamage(float damage, float knockBackPower, float knockBackTime, float freezeTime, Vector3 enemyPos)
    {
        rb.velocity = Vector2.zero;
        GameManager.instance.ChangeStat("health", -damage);
        AudioManager.instance.Play("hit_sound", "Once");
        if (isDashing)
        {
            DashTimer = 0;
            isDashing = false;
            AfterImageScript.makeImage = false;
            rb.velocity = Vector2.zero;
        }
        FreezeTimer = freezeTime;
        KnockTimer = knockBackTime;
        CameraController.Shake(0.25f, 0.1f);

        if (GameManager.instance.health <= 0)
        {
            GameManager.instance.health = 0;
            Die();
        }
        else
        {
            spriteRenderer.color = Color.red;
            isHit = true;
         
            Vector2 dir = rb.transform.position - enemyPos;

            Debug.DrawRay(transform.position, new Vector2(dir.normalized.x, 1f) * knockBackPower, Color.white, 5f);

            dir = new Vector2(dir.x, 0);
            if (!grounded)
            {
                rb.velocity = new Vector2(dir.normalized.x, 0.3f) * knockBackPower;
            }
            else
            {
                rb.velocity = new Vector2(dir.normalized.x, 1) * knockBackPower;
            }
        }
    }

    public void Die()
    {
        Debug.Log("working");
        rb.isKinematic = true;
        boxcollider.enabled = false;
        rb.velocity = Vector2.zero;
        isDead = true;
        anim.SetTrigger("isDead");
        var script = GetComponent<CharacterController2D>();
        script.enabled = false;
    }
    
    public void DieAnim()
    {
        GameManager.instance.Animate("death");
    }

    private void CreateDust()
    {
        dust.Play();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }
}
