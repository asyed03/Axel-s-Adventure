using UnityEngine.SceneManagement;
using UnityEditor;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Loadtest : MonoBehaviour
{
    public void LoadLevel()
    {
        StartCoroutine(LoadSceneAsynchronous("Test"));
        Debug.Log("failure");
    }
    public IEnumerator LoadSceneAsynchronous(string level)
    {
        string SceneName = "";
        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            if (EditorBuildSettings.scenes[i].path == "Assets/Scenes/" + level + ".unity")
            {
                SceneName = level;
                break;
            }
        }
        if (SceneName == "")
        {
            Debug.LogError("Scene Not Found!");
            yield break;
        }
        SceneManager.LoadScene("Loading Screen");
        Debug.Log(level);
        yield return new WaitForSeconds(1);
        Image loadingBar = GameObject.FindGameObjectWithTag("LoadBar").GetComponent<Image>();
        AsyncOperation loading = SceneManager.LoadSceneAsync(level);
        while (!loading.isDone)
        {
            Debug.Log("YEES");
            if (loadingBar != null)
                loadingBar.fillAmount = loading.progress;
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
    }
}
