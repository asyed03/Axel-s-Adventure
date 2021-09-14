using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerGetter : MonoBehaviour
{
    #region  GAMEMANGER FUNCTIONS

    public void TogglePauseGame()
    {
        GameManager.instance.TogglePauseGame();
    }

    public void LoadMainMenu()
    {
        GameManager.instance.LoadMainMenu();
    }

    public void LoadSceneFromPauseMenu(string scene)
    {
        GameManager.instance.LoadSceneFromPauseMenu(scene);
    }

    public void LoadLevel(int levelnum)
    {
        GameManager.instance.LoadLevel(levelnum);
    }

    public void ReloadLevel()
    {
        GameManager.instance.ReloadLevel();
    }

    public void LoadNextLevel()
    {
        GameManager.instance.LoadNextLevel();
    }

    public void LoadScene(string scene)
    {
        GameManager.instance.LoadScene(scene);
    }

    public void UnloadScene(string scene)
    {
        GameManager.instance.UnloadScene(scene);
    }

    public void LoadPrevScene()
    {
        GameManager.instance.LoadPrevScene();
    }

    public void LoadNextScene()
    {
        GameManager.instance.LoadNextScene();
    }

    public void ResetGame()
    {
        GameManager.instance.ResetGame();
    }

    public void SaveGame()
    {
        GameManager.instance.SaveData();
    }

    public void LoadGame()
    {
        GameManager.instance.LoadData();
    }

    public void QuitGame()
    {
        GameManager.instance.QuitGame();
    }

    #endregion

    #region AUDIOMANAGER FUNCTIONS

    public void Play(string name, string option)
    {
        AudioManager.instance.Play(name, option);
    }

    #endregion

    #region DIALOGUEMANAGER FUNCTIONS

    public void CloseDialogue()
    {
        DialogueManager.instance.slideout();
    }

    #endregion
}

