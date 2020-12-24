using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    public GameObject target;
    public PlayableDirector timeline;
    public enum TargetType {Player, Enemy, Static };
    public TargetType targetType;

    public bool triggered = false;

    private void OnEnable()
    {
        timeline.stopped += EndCutscene;  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && !triggered)
        {
            Debug.Log("cutscene triggered");
            TriggerCutscene();
        }
    }

    void TriggerCutscene()
    {
        GameManager.instance.pauseable = false;
        GameManager.instance.GetComponentInChildren<Effects>().Animate("cutsceneIn");
        triggered = true;
        switch (targetType)
        {
            case TargetType.Player:
                var player = target.GetComponent<CharacterController2D>();
                player.rb.velocity = Vector2.zero;
                player.rb.isKinematic = true;
                player.cutscene = true;
                timeline.Play();
                break;

            case TargetType.Enemy:
                var enemy = target.GetComponent<EnemyController>();
                enemy.rb.velocity = Vector2.zero;
                enemy.rb.isKinematic = true;
                enemy.frozen = true;
                timeline.Play();
                break;

            default:
                break;
        }
    }

    void EndCutscene(PlayableDirector p)
    {

        if (GameManager.instance != null)
        {
            GameManager.instance.GetComponentInChildren<Effects>().Animate("cutsceneOut");
        }
        Debug.Log("Done playing!");
        switch (targetType)
        {
            case TargetType.Player:
                var player = target.GetComponent<CharacterController2D>();
                player.rb.isKinematic = false;
                player.frozen = false;
                player.cutscene = false;
                break;

            case TargetType.Enemy:
                var enemy = target.GetComponent<EnemyController>();
                enemy.rb.isKinematic = false;
                enemy.frozen = false;
                break;

            default:
                break;
        }
    }  

    private void OnDisable() 
    {
        timeline.stopped -= EndCutscene;
    }
}

