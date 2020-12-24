using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBounds : MonoBehaviour
{
    public BoxCollider2D Levelbounds;
    // Start is called before the first frame update
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Mathf.Abs(Levelbounds.bounds.center.y - collision.gameObject.transform.position.y) >= Mathf.Abs(Levelbounds.bounds.center.y - Levelbounds.bounds.extents.y) ||
               Mathf.Abs(Levelbounds.bounds.center.y - collision.gameObject.transform.position.x) >= Mathf.Abs(Levelbounds.bounds.center.x - Levelbounds.bounds.extents.x))
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                collision.gameObject.GetComponent<CharacterController2D>().Die();
            }
            else
            {

                Debug.Log(gameObject.name + "destroyed!");
                Destroy(collision.gameObject, 0.3f);
            }
        }
    }
}
