using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateUI : MonoBehaviour
{
    public TextMeshProUGUI CoinText;
    public TextMeshProUGUI LevelText;
    public GameObject pauseMenu;
    public GameObject puaseButton;
    public RectTransform healthScale;
    public RectTransform healthTotal;

    // Start is called before the first frame update
    void Start()
    {
        if (!GameManager.instance.GamePaused)
        {
            pauseMenu.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CoinText.text = "Coins: " + GameManager.instance.coins;
        LevelText.text = "Level: " + GameManager.instance.Levels[GameManager.instance.currentLevel - 1].number;
        healthScale.localScale = new Vector3(((float)GameManager.instance.health / (float)GameManager.instance.maxHealth) * healthTotal.localScale.x, healthScale.localScale.y, healthScale.localScale.z);
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameManager.instance.TogglePauseGame();
        }

        if (GameManager.instance.GamePaused && !pauseMenu.activeSelf)
        {
            pauseMenu.SetActive(true);
            puaseButton.SetActive(false);
        }
        else if (!GameManager.instance.GamePaused && pauseMenu.activeSelf)
        {
            pauseMenu.SetActive(false);
            puaseButton.SetActive(true);
        }
    }
}
