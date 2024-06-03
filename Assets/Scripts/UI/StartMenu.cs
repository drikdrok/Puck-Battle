using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{


    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI enemyNameText;

    public GameObject StartScreen;
    public GameObject VersusScreen;
    public GameObject SettingsScreen;


    void Start()
    {

        VersusScreen.SetActive(false);
        SettingsScreen.SetActive(false);
        StartScreen.SetActive(true);
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void SelectPlayer(string player)
    {
        playerNameText.text = player;
        StartManager.playerName = player;
    }

    public void SelectEnemy(string enemy)
    {
        enemyNameText.text = enemy;
        StartManager.enemyName = enemy;
    }

    public void ShowVersusScreen()
    {
        StartScreen.SetActive(false);
        VersusScreen.SetActive(true);
    }

    public void ShowSettingsScreen()
    {
        SettingsScreen.SetActive(true);
        StartScreen.SetActive(false);
    }

    public void HideSettingsScreen()
    {
        SettingsScreen.SetActive(false);
        StartScreen.SetActive(true);
    }

    public void ResetEverything()
    {
        PlayerPrefs.SetInt("DoTutorial", 1);
    }


    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
