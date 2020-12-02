using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelsUpdater : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var Buttons = GetComponentsInChildren<Button>();
        foreach (var b in Buttons)
        {
            if (!GameManager.instance.Levels[int.Parse(b.name)-1].unlocked)
            {
                b.interactable = false;
                b.image.color = Color.red;
            }
            else
            {
                b.interactable = true;
                b.image.color = Color.white;
            }
        }
    }
}
