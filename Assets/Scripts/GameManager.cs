﻿using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float health;
    public float coins;
    public int currentLevel = 0;
    public float jumpMultiplier;
    public float speedMultiplier;
    public bool GamePaused = false;
    public Animator anim;
    public Level[] Levels;
    public string lastSceneLoaded = "MainMenu";
    public GameObject maskBG;
    public GameObject circleMask;
    public Canvas EffectsCanvas;
    public bool LevelLoaded = false;

    void Awake()
    {
        MakeSingleton();

        InititializeGameDefault();

        LoadMainMenu();
    }

    void Start()
    {
        maskBG.SetActive(false);
        circleMask.SetActive(false);
        SetMixerVolume();
    }

    void MakeSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void InititializeGameDefault()
    {

        if (PlayerPrefs.GetInt("level1", 0) != 1)
        {
            PlayerPrefs.SetInt("level1", 1);
        }

        for (int i = 0; i < Levels.Count(); i++)
        {
            Levels[i].unlocked = PlayerPrefs.GetInt("level" + (i + 1), 0) == 1;
            Levels[i].number = i + 1;
        }
    }

    public void SetMixerVolume ()
    {
        AudioManager.instance.audioMixer.SetFloat("volumeMusic", Mathf.Log10(PlayerPrefs.GetFloat("volumeMusic")) * 20);
        AudioManager.instance.audioMixer.SetFloat("volumeSFX", Mathf.Log10(PlayerPrefs.GetFloat("volumeSFX")) * 20);
    }

    public void ChangeStat(string s, float i)
    {
        switch (s)
        {
            case "health":
                health += i;
                break;

            case "coins":
                coins += i;
                AudioManager.instance.Play("coin_sound", "Once");
                break;

            case "jumpMultiplier":
                jumpMultiplier += i;
                break;

            case "speedMultiplier":
                jumpMultiplier += i;
                break;
        }
    }

    public void TogglePauseGame()
    {
        if (!GamePaused)
        {
            GamePaused = !GamePaused;
            Time.timeScale = 0;
        }
        else
        {
            GamePaused = !GamePaused;
            Time.timeScale = 1;
        }
    }

    public void LoadMainMenu()
    {
        if (GamePaused)
        {
            TogglePauseGame();
        }
        SceneManager.LoadScene("MainMenu");
        LevelLoaded = false;
        AudioManager.instance.Play("menu_music", "Continuous");
    }

    public void LoadSceneFromPauseMenu(string scene)
    {
        SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
    }

    public void LoadLevel(int levelnum)
    {
        //health = 100;
        lastSceneLoaded = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(Levels[levelnum - 1].scene.name);
        currentLevel = levelnum;
        if (!Levels[currentLevel - 1].unlocked || PlayerPrefs.GetInt("level" + currentLevel, 0) != 1)
        {
            Levels[currentLevel - 1].unlocked = true;
            PlayerPrefs.SetInt("level" + currentLevel, 1);
        }
        LevelLoaded = true;
        AudioManager.instance.Play("level_music", "Continuous");
    }

    public void ReloadLevel()
    {
        //health = 100;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (GamePaused)
        {
            TogglePauseGame();
        }
        LevelLoaded = true;
        AudioManager.instance.Play("level_music", "Continuous");
    }

    public void LoadNextLevel()
    {
        //health = 100;
        SceneManager.LoadScene(Levels[currentLevel].scene.name);
        LevelLoaded = true;
        AudioManager.instance.Play("level_music", "Continuous");
        currentLevel += 1;
        Levels[currentLevel - 1].unlocked = true;
        PlayerPrefs.SetInt("level" + currentLevel, 1);
    }

    public void LoadScene(string scene)
    {
        lastSceneLoaded = SceneManager.GetActiveScene().name;
        LevelLoaded = false;
        SceneManager.LoadScene(scene);
    }

    public void UnloadScene(string scene)
    {
        SceneManager.UnloadSceneAsync(scene);
    }

    public void LoadPrevScene()
    {
        foreach (Level l in Levels)
        {
            if (l.scene.name == lastSceneLoaded)
            {
                LevelLoaded = true;
                break;
            }
        }

        string temp = SceneManager.GetActiveScene().name;
        if (GamePaused || temp == "OptionsMenu2")
        {
            Time.timeScale = 0;
            SceneManager.UnloadSceneAsync("OptionsMenu2");
        }
        else
        {
            SceneManager.LoadScene(lastSceneLoaded);
            //health = 100;
        }
        lastSceneLoaded = temp;
    }

    public void LoadNextScene()
    {
        lastSceneLoaded = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void DeathAnimate()
    {
        circleMask.SetActive(true);
        maskBG.SetActive(true);
        StartCoroutine(ChangeColor(1f));
        maskBG.GetComponent<Image>().color = new Color(maskBG.GetComponent<Image>().color.r, maskBG.GetComponent<Image>().color.g, maskBG.GetComponent<Image>().color.b, 255);
        circleMask.transform.position = GameObject.Find("Player").transform.position;
        circleMask.GetComponent<Animator>().Play("Death");
    }

    public void ResetGame()
    {
        PlayerPrefs.DeleteAll();
        InititializeGameDefault();
        SetMixerVolume();
        LoadMainMenu();
    }

    public void GameOver()
    {
        circleMask.SetActive(false);
        maskBG.SetActive(false);
        LoadScene("GameOver");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator ChangeColor(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        
    }
}
