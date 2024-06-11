using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{


    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI playerDescriptionText;
    public TextMeshProUGUI playerUnlockText;
    public TextMeshProUGUI playerWinsText;
    public TextMeshProUGUI playerLossesText;
    public Button playerSelectButton;

    Dictionary<string, string> descriptions;
    Dictionary<string, string> unlockCriteria;

    public TextMeshProUGUI enemyNameText;
    public TextMeshProUGUI enemyDescriptionText;
    public TextMeshProUGUI enemyWinsText;
    public TextMeshProUGUI enemyLossesText;
    public TextMeshProUGUI enemyOvertimeText;
    public TextMeshProUGUI enemyMatchupText;


    public GameObject StartScreen;
    public GameObject SettingsScreen;

    public GameObject PlayerSelect;
    public GameObject EnemySelect;

    public GameObject mummyLock;
    public GameObject vikingLock;
    public GameObject yetiLock;

    Dictionary<string, bool> isUnlocked;

    void Start()
    {

        descriptions = new Dictionary<string, string>() {
            { "Joe", "Just an average Joe"},
            { "Mummy", "A relic of an age long gone, yet still haunting the present"},
            { "Viking", "The northern brute - his pucks turn to fire!" },
            { "Yeti", "An Arctic Anomaly" }
        };

        unlockCriteria = new Dictionary<string, string>() {
            { "Joe", ""},
            { "Mummy", "Defeat the Mummy 3 times to unlock"},
            { "Viking", "Defeat the Viking and Yeti in overtime to unlock" },
            { "Yeti", "Defeat every enemy to unlock" }
        };

        isUnlocked = new Dictionary<string, bool>() {
            { "Joe", true},
            { "Mummy", false},
            { "Viking", false},
            { "Yeti", false}
        };



        PlayerSelect.SetActive(false);
        EnemySelect.SetActive(false);
        SettingsScreen.SetActive(false);
        StartScreen.SetActive(true);

        checkLocks();
        SelectPlayer("Joe");
        
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

        playerDescriptionText.text = descriptions[player];
        playerUnlockText.gameObject.SetActive(true);
        playerUnlockText.text = unlockCriteria[player];
        playerWinsText.text = PlayerPrefs.GetInt("winsAs" + player).ToString();
        playerLossesText.text = PlayerPrefs.GetInt("defeatsAs" + player).ToString();

        if (isUnlocked[player])
        {
            playerSelectButton.interactable = true;
        }
        else
        {
            playerSelectButton.interactable = false;
        }


    }

    private void checkLocks()
    {

        int winsAgainstJoe = PlayerPrefs.GetInt("winsAgainstJoe");
        int winsAgainstMummy = PlayerPrefs.GetInt("winsAgainstMummy");
        int winsAgainstViking = PlayerPrefs.GetInt("winsAgainstViking");
        int winsAgainstYeti = PlayerPrefs.GetInt("winsAgainstYeti");

        bool hasOvertimeWinJoe = PlayerPrefs.GetInt("hasOvertimeWinJoe") > 0 ? true : false;
        bool hasOvertimeWinMummy = PlayerPrefs.GetInt("hasOvertimeWinMummy") > 0 ? true : false;
        bool hasOvertimeWinViking = PlayerPrefs.GetInt("hasOvertimeWinViking") > 0 ? true : false;
        bool hasOvertimeWinYeti = PlayerPrefs.GetInt("hasOvertimeWinYeti") > 0 ? true : false;

        if (PlayerPrefs.GetInt("winsAgainstMummy") >= 3)
        {
            isUnlocked["Mummy"] = true;
            mummyLock.SetActive(false);
        }
        else
        {
            mummyLock.SetActive(true);
        }
        if (hasOvertimeWinViking && hasOvertimeWinYeti)
        {
            isUnlocked["Viking"] = true;
            vikingLock.SetActive(false);
        }
        else
        {
            vikingLock.SetActive(true);
        }
        if (winsAgainstJoe >= 1 && winsAgainstMummy >= 1 && winsAgainstViking >= 1 && winsAgainstYeti >= 1)
        {
            isUnlocked["Yeti"] = true;
            yetiLock.SetActive(false);
        }
        else
        {
            yetiLock.SetActive(true);
        }
    }

    public void UnlockAll()
    {
        isUnlocked["Mummy"] = true;
        mummyLock?.SetActive(false);
        isUnlocked["Viking"] = true;
        vikingLock.SetActive(false);
        isUnlocked["Yeti"] = true;
        yetiLock?.SetActive(false);
    }

    public void SelectEnemy(string enemy)
    {
        enemyNameText.text = enemy;
        StartManager.enemyName = enemy;

        enemyDescriptionText.text = descriptions[enemy];
        enemyWinsText.text = PlayerPrefs.GetInt("winsAgainst" + enemy).ToString();
        enemyLossesText.text = PlayerPrefs.GetInt("defeatsAgainst" + enemy).ToString();
        enemyOvertimeText.text = PlayerPrefs.GetInt("hasOvertimeWin" + enemy) == 1 ? "Yes" : "No";

        enemyMatchupText.text = StartManager.playerName + " V " + enemy;
    }

    public void ShowPlayerSelect()
    {
        StartScreen.SetActive(false);
        EnemySelect.SetActive(false);
        PlayerSelect.SetActive(true);
    }
    public void ShowEnemySelect()
    {
        EnemySelect.SetActive(true);
        PlayerSelect.SetActive(false);
        SelectEnemy("Joe");
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
        PlayerPrefs.DeleteAll();
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
