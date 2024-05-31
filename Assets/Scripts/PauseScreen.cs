using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] TextMeshProUGUI timerText;

    [SerializeField] Transform panel;


    public bool paused = false;

    private void Start()
    {
        gameManager = GetComponent<GameManager>();
        panel.gameObject.SetActive(false);

    }
    public void PauseGame()
    {
        paused = true;
        timerText.text = "Paused";
        panel.gameObject.SetActive(true);
        gameManager.DisablePlayers();
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        paused = false;
        panel.gameObject.SetActive(false);
        gameManager.StartCountdown();

    }


    public void ExitGame()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
