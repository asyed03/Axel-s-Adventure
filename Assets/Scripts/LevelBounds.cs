using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBounds : MonoBehaviour
{
    public BoxCollider2D Levelbounds;
    // Start is called before the first frame update
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.GetComponent<CharacterController2D>().Die();
        }
        else
        {
            if (collision.gameObject.transform.position.y <= Levelbounds.bounds.min.y)
            {
                Debug.Log(collision.gameObject.name + " destroyed!");
                Destroy(collision.gameObject, 0.3f);
            }
        }
    }
}
