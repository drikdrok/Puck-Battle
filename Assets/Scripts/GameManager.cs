using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    float countDownSpeed = 0.4f;

    [SerializeField] TextMeshProUGUI playerNameText;
    [SerializeField] TextMeshProUGUI enemyNameText;

    [SerializeField] RawImage playerPortrait;
    [SerializeField] RawImage enemyPortrait;

    [SerializeField] Texture[] portraitSprites;


    [SerializeField] TextMeshProUGUI countDownText;

    [SerializeField] TextMeshProUGUI timerText;

    [SerializeField] GameObject player;
    [SerializeField] GameObject enemy;

    float countDownTime = 3;
    bool countDownFadeOut = false;

    int minutes = 1;
    float seconds = 0;

    public bool doingTutorial = true;
    [SerializeField] Tutorial tutorial;

    PauseScreen pauseScreen;
    [SerializeField] GameObject endScreen;

    bool overtime = false;

    bool hasDoneStats = false;

    void Start()
    {

        pauseScreen = GetComponent<PauseScreen>();

        playerNameText.text = StartManager.playerName;
        enemyNameText.text= StartManager.enemyName;

        DisablePlayers();

        SetPortrait(StartManager.playerName, playerPortrait);
        SetPortrait(StartManager.enemyName, enemyPortrait);

        StartCountdown();


        endScreen.SetActive(false);

        if (PlayerPrefs.GetInt("DoTutorial") == 1)
        {
            doingTutorial = true;
            PlayerPrefs.SetInt("DoTutorial", 0);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (countDownFadeOut)
        {
            countDownText.rectTransform.position += Vector3.up * 100 * Time.deltaTime;
            Color newColor = countDownText.color;
            newColor.a -=  2 * Time.deltaTime;
            countDownText.color = newColor;
        }


        if (Input.GetKeyDown(KeyCode.Escape) && countDownTime <= 0)
        {
            if (pauseScreen.paused)
            {
                pauseScreen.ResumeGame();
            }
            else
            {
                pauseScreen.PauseGame();
            }
        }
        DoOvertime();
    }

    public void StartCountdown()
    {
        StopAllCoroutines();
        countDownTime = 3;
        countDownFadeOut = false;
        countDownText.text = "3";
        countDownText.rectTransform.position = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f); ;
        countDownText.color= Color.white;

        StartCoroutine(DoCountDown());
    }

    public void DisablePlayers()
    {
        player.SetActive(false);
        enemy.SetActive(false);
    }

    IEnumerator DoCountDown()
    {
        yield return new WaitForSecondsRealtime(countDownSpeed);
        countDownTime -= 1;
        countDownText.text = Mathf.Ceil(countDownTime).ToString();
        if (countDownTime > 0)
        {
            StartCoroutine(DoCountDown());
        }
        else
        {
            countDownFadeOut = true;
            countDownText.text = "GO!";

            if (doingTutorial) 
            {
                tutorial.gameObject.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }

            string secondsText = seconds.ToString();
            if (seconds <= 9) secondsText = "0" + secondsText;

            timerText.text = minutes.ToString() + ":" + secondsText;

            player.SetActive(true);
            enemy.SetActive(true);
            StartCoroutine(DoTimer());
        }
    }

    IEnumerator DoTimer()
    {
        yield return new WaitForSeconds(1);
        if (seconds == 0)
        {
            if (minutes == 0) // Game is over
            {

                int playerScore = player.GetComponent<ScoreHandler>().score;
                int enemyScore = enemy.GetComponent<ScoreHandler>().score;

                if (playerScore == enemyScore)
                {
                    overtime = true;
                }else if (playerScore > enemyScore)
                {
                    StatsWin(false);
                    endScreen.SetActive(true);
                    Time.timeScale = 0;
                    timerText.text = "You Win!";
                }
                else
                {
                    StatsLose();
                    endScreen.SetActive(true);
                    Time.timeScale = 0;
                    timerText.text = "You Lose!";
                }

                yield break;
            }
            
            minutes--;
            seconds = 59;
            
        }
        else
        {
            seconds--;
        }

        string secondsText = seconds.ToString();
        if (seconds <= 9) secondsText = "0" + secondsText;

        timerText.text = minutes.ToString() + ":" + secondsText;
        StartCoroutine(DoTimer());
    }


    void SetPortrait(string character, RawImage img)
    {
        if (character == "Joe")
        {
            img.texture = portraitSprites[0];
        }
        else if (character == "Mummy")
        {
            img.texture = portraitSprites[1];
        }
        else if (character == "Viking")
        {
            img.texture = portraitSprites[2];
        }
        else if (character == "Yeti")
        {
            img.texture = portraitSprites[3];
        }
        else
        {
            img.texture = portraitSprites[0];
        }
    }


    public void dragPuck()
    {
        if (doingTutorial)
        {
            if (!pauseScreen.paused)
            {
                tutorial.dragPuck();
                Time.timeScale = 1;
            }
        }
    }

    public void shootPuck()
    {
        if (doingTutorial)
        {
            tutorial.shootPuck();
        }
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("SampleScene");
    }


    private void DoOvertime()
    {
        if (overtime)
        {
            int playerScore = player.GetComponent<ScoreHandler>().score;
            int enemyScore = enemy.GetComponent<ScoreHandler>().score;

            if (enemyScore > playerScore)
            {
                endScreen.SetActive(true);
                Time.timeScale = 0;
                timerText.text = "You Lose!";
                StatsLose();
            }
            else if (enemyScore < playerScore)
            {
                endScreen.SetActive(true);
                Time.timeScale = 0;
                timerText.text = "You Win!";
                StatsWin(true);
            }
            else
            {
                timerText.text = "Overtime!";
            }
        }
    }


    private void StatsWin(bool overtime)
    {
        if (hasDoneStats) { return; }
        hasDoneStats = true;
        PlayerPrefs.SetInt("winsAs" + StartManager.playerName, PlayerPrefs.GetInt("winsAs" + StartManager.playerName) + 1);
        PlayerPrefs.SetInt("winsAgainst" + StartManager.enemyName, PlayerPrefs.GetInt("winsAgainst" + StartManager.enemyName) + 1);
        if (overtime)
        {
            PlayerPrefs.SetInt("hasOvertimeWin" + StartManager.enemyName, 1);
        }
    }

    private void StatsLose()
    {
        if (hasDoneStats) { return; }
        hasDoneStats = true;
        PlayerPrefs.SetInt("defeatsAs" + StartManager.playerName, PlayerPrefs.GetInt("defeatsAs" + StartManager.playerName) + 1);
        PlayerPrefs.SetInt("defeatsAgainst" + StartManager.enemyName, PlayerPrefs.GetInt("defeatsAgainst" + StartManager.enemyName) + 1);
    }
}
