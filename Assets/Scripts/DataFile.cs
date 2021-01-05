using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataFile
{
    public int maxHealth;
    public int coins;
    public Level[] levels;
    //Stats
    public float dashMultiplier;
    public float attackMultiplier;
    public float speedMultiplier;
    //Audio settings
    public float masterLevel;
    public float musicLevel;
    public float sfxLevel;
    public DataFile (GameManager game)
    {
        maxHealth = game.maxHealth;
        coins = game.coins;
        levels = game.Levels;
        dashMultiplier = game.dashMultiplier;
        attackMultiplier = game.attackMultiplier;
        speedMultiplier = game.speedMultiplier;

        masterLevel = game.masterLevel;
        musicLevel = game.musicLevel;
        sfxLevel = game.sfxLevel;  
    }
}
