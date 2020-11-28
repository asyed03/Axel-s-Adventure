using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public enum MoveType{Horizontal, Vertical};
    public MoveType moveType;
    public float speed;
    public float moveRange;
    public bool active = false;
    public bool crumble = false;
    public float crumbleTime = 0f;

    public Collider2D col;
    private Vector2 startPosition;
    private Vector2 moveDir;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    private void OnEnable()
    {
        if (moveType == MoveType.Vertical)
        {
            moveDir = Vector2.up;
        }
        else
        {
            moveDir = Vector2.right;
        }
        if (active)
        {
            rb.velocity = moveDir;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void Update()
    {
        if (transform.position.x >= startPosition.x + moveRange || transform.position.x <= startPosition.x - moveRange || transform.position.y >= startPosition.y + moveRange || transform.position.y <= startPosition.y - moveRange)
        {
            Flip();
        }
    }

    void Flip()
    {
        rb.velocity = moveDir *= -1;
    }
    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (LayerMask.LayerToName(collider.gameObject.layer) == "Player" && crumble)
        {
            StartCoroutine(Crumble(crumbleTime));
        }
    }
    private IEnumerator Crumble(float waitTime)
    {
        Collider2D[] cols = GetComponents<Collider2D>();
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        yield return new WaitForSeconds(waitTime);
        sprite.enabled = false;
        foreach (Collider2D col in cols)
        {
            col.enabled = false;
        }
        yield return new WaitForSeconds(waitTime);
        sprite.enabled = true;
        foreach (Collider2D col in cols)
        {
            col.enabled = true;
        }
    }

    public void ToggleState()
    {
        if (active)
        {
            rb.velocity = Vector2.zero;
            active = !active;
        }
        else
        {
            rb.velocity = speed * moveDir;
            active = !active;
        }
    }
}
