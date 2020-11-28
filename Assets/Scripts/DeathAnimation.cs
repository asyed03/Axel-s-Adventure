using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    public void GameOver ()
    {
        GameManager.instance.GameOver();
    }
}
