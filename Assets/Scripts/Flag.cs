using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public GameObject levelCompleteUI;
    public CharacterController2D player;

    private void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();
        levelCompleteUI.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            LevelComplete();
        }
    }

    public void LevelComplete()
    {
        Debug.Log("finished level");
        levelCompleteUI.SetActive(true);
        levelCompleteUI.GetComponent<Animator>().SetTrigger("slidein");
        GameManager.instance.pauseable = false;
        player.frozen = true;
    }

    IEnumerator LevelBox()
    {
        levelCompleteUI.GetComponent<Animator>().SetTrigger("slidein");
        yield return new WaitForSeconds(1f);
    }
}
