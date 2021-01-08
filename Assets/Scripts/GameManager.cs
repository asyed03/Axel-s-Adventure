using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int health;
    public int maxHealth;
    public int coins;
    public int currentLevel = 0;
    public float speedMultiplier;
    public float jumpMultiplier;
    public float dashMultiplier;
    public float attackMultiplier;
    public float masterLevel = 1;
    public float musicLevel = 1;
    public float sfxLevel = 1;
    public bool GamePaused = false;
    public bool pauseable = true;
    public Animator anim;
    public Level[] Levels;
    public string lastSceneLoaded = "MainMenu";
    public bool LevelLoaded = false;
    public Effects effects;
    void Awake()
    {
        MakeSingleton();

        InititializeGameDefault();    
    }

    private void OnEnable()
    {
        for (int i = 0; i < Levels.Length; i++)
        {
            Levels[i].number = i;
        }
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "_preload")
        {
            LoadMainMenu();
        }
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
        Debug.Log(SaveSystem.path);
        if (!File.Exists(SaveSystem.path)) 
        {
            Debug.Log("NewGame");
            health = 100;
            maxHealth = 100;
            coins = 0;
            speedMultiplier = 1;
            jumpMultiplier = 1;
            dashMultiplier = 1;
            attackMultiplier = 1;
            masterLevel = 1;
            musicLevel = 1;
            sfxLevel = 1;
            Levels[0].unlocked = true;
            for (int i = 1; i < Levels.Length; i++)
            {
                Levels[i].score = 0;
                Levels[i].time = 0;
                Levels[i].unlocked = false;
            }
            SaveData();
        }
        else
        {
            Debug.Log("data loaded");
            LoadData();
        }
        /*
        if (PlayerPrefs.GetInt("level1", 0) != 1)
        {
            PlayerPrefs.SetInt("level1", 1);
        }

        maxHealth = PlayerPrefs.GetInt("maxHealth", 100);
        health = maxHealth;
        coins = PlayerPrefs.GetInt("coins", 0);
        speedMultiplier = PlayerPrefs.GetFloat("speedMultiplier", 1);
        jumpMultiplier = PlayerPrefs.GetFloat("jumpMultiplier", 1);
        dashMultiplier = PlayerPrefs.GetFloat("dashMultiplier", 1);

        for (int i = 0; i < Levels.Count(); i++)
        {
            Levels[i].unlocked = PlayerPrefs.GetInt("level" + (i + 1), 0) == 1;
            Levels[i].number = i + 1;
        }
        */
    }

    public void SetMixerVolume ()
    {
        AudioManager.instance.audioMixer.SetFloat("volumeMusic", musicLevel);
        AudioManager.instance.audioMixer.SetFloat("volumeSFX", sfxLevel);
    }

    public void ChangeStat(string s, float i, bool save = false)
    {
        switch (s)
        {
            case "health":
                health += Mathf.FloorToInt(i);
                break;

            case "maxHealth":
                maxHealth = Mathf.FloorToInt(i);
                    PlayerPrefs.SetInt("maxHealth", maxHealth);
                break;

            case "coins":
                coins += Mathf.FloorToInt(i);
                AudioManager.instance.Play("coin_sound", "Once");
                if (save)
                    PlayerPrefs.SetInt("coins", coins);
                break;

            case "speedMultiplier":
                speedMultiplier += i;
                if (save)
                    PlayerPrefs.SetFloat("speedMultiplier", speedMultiplier);
                break;

            case "jumpMultiplier":
                jumpMultiplier += i;
                if (save)
                    PlayerPrefs.SetFloat("jumpMultiplier", jumpMultiplier);
                break;

            case "dashMultiplier":
                dashMultiplier += i;
                if (save)
                    PlayerPrefs.SetFloat("dashMultiplier", dashMultiplier);
                break;

            case "attackMultiplier":
                dashMultiplier += i;
                if (save)
                    PlayerPrefs.SetFloat("attackMultiplier", dashMultiplier);
                break;
        }
    }

    public void TogglePauseGame()
    {
        if (pauseable)
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
    }

    public void LoadMainMenu()
    {
        if (GamePaused)
        {
            TogglePauseGame();
        }
        SceneManager.LoadScene("MainMenu");
        AudioManager.instance.Play("menu_music", "Continuous");
        LevelLoaded = false;
    }

    public void LoadSceneFromPauseMenu(string scene)
    {
        SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
    }

    public void LoadLevel(int levelnum)
    {
        //health = 100;
        lastSceneLoaded = SceneManager.GetActiveScene().name;
        StartCoroutine(LoadSceneAsynchronous(Levels[levelnum - 1].sceneName));
        //SceneManager.LoadSceneAsync(Levels[levelnum - 1].sceneName);
        currentLevel = levelnum;
        if (!Levels[currentLevel - 1].unlocked || PlayerPrefs.GetInt("level" + currentLevel, 0) != 1)
        {
            Levels[currentLevel - 1].unlocked = true;
            PlayerPrefs.SetInt("level" + currentLevel, 1);
        }
        LevelLoaded = true;
        AudioManager.instance.Play("level_music", "Continuous");
    }

    public IEnumerator LoadSceneAsynchronous(string level)
    {
        GameManager.instance.SaveData();

        //fade animation stuff
        pauseable = false;
        effects.fadeoutMask.SetActive(true);
        effects.fadeoutMask.GetComponent<Animator>().SetTrigger("in");
        yield return new WaitForSeconds(1f);
        //
        AsyncOperation load = SceneManager.LoadSceneAsync("Loading Screen");
        SaveData();
        LoadData();
        while (!load.isDone)
        {
            yield return null;
        }
        //fade animation stuff
        effects.fadeoutMask.SetActive(false);
        Debug.Log("loading");
        Debug.Log(level);
        yield return new WaitForSeconds(1f);
        //
        Image loadingBar = GameObject.FindGameObjectWithTag("LoadBar").GetComponent<Image>();
        AsyncOperation loading = SceneManager.LoadSceneAsync(level);
        while (!loading.isDone)
        {
            if (loadingBar != null)
                loadingBar.fillAmount = loading.progress;
            yield return null;
        }
        //fade animation stuff
        effects.fadeoutMask.SetActive(true);
        effects.fadeoutMask.GetComponent<Animator>().SetTrigger("out");
        yield return new WaitForSeconds(1f);
        effects.fadeoutMask.SetActive(false);
        pauseable = true;
        //
    }
    public void ReloadLevel()
    {
        //health = 100;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (GamePaused)
        {
            TogglePauseGame();
        }
        pauseable = true;
        LevelLoaded = true;
        AudioManager.instance.Play("level_music", "Continuous");
    }

    public void LoadNextLevel()
    {
        //health = 100;
        StartCoroutine(LoadSceneAsynchronous(Levels[currentLevel].sceneName));
        LevelLoaded = true;
        AudioManager.instance.Play("level_music", "Continuous");
        currentLevel += 1;
        Levels[currentLevel - 1].unlocked = true;
        PlayerPrefs.SetInt("level" + currentLevel, 1);
        pauseable = true;
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
            if (l.sceneName == lastSceneLoaded)
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
    /*
    public void DeathAnimate()
    {
        circleMask.SetActive(true);
        maskBG.SetActive(true);
        maskBG.GetComponent<Image>().color = new Color(maskBG.GetComponent<Image>().color.r, maskBG.GetComponent<Image>().color.g, maskBG.GetComponent<Image>().color.b, 255);
        circleMask.transform.position = GameObject.Find("Player").transform.localPosition;
        circleMask.transform.position = new Vector3(circleMask.transform.position.x, circleMask.transform.position.y + 0.5f, circleMask.transform.position.z);
        circleMask.GetComponent<Animator>().Play("Death");
    }
    */


    public void Animate(string s)   
    {
        GetComponentInChildren<Effects>().Animate(s);
    }
    public void ResetGame()
    {
        File.Delete(SaveSystem.path);
        InititializeGameDefault();
        SetMixerVolume();
        LoadMainMenu();
    }

    public void GameOver()
    {
        LoadScene("GameOver");
    }

    public void SaveData()
    {
        SaveSystem.SaveData(this);
    }

    public void LoadData()
    {
        DataFile data = SaveSystem.LoadData();

        maxHealth = data.maxHealth; 
        coins = data.coins;
        Levels = data.levels;
        dashMultiplier = data.dashMultiplier;
        attackMultiplier = data.attackMultiplier;
        speedMultiplier = data.speedMultiplier;

        masterLevel = data.masterLevel;
        musicLevel = data.musicLevel;
        sfxLevel = data.sfxLevel;
    }
    public void QuitGame()
    {
        GameManager.instance.SaveData();
        Application.Quit();
    }
}
