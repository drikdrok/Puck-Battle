using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource playerScore;
    public AudioSource enemyScore;
    public AudioSource puckCollide;
    public AudioSource puckWallImpact;

    public AudioSource joePowerup;
    public AudioSource mummyPowerup;
    public AudioSource vikingPowerUp;
    public AudioSource yetiPowerup;

    public AudioSource vikingShoot;

    public AudioSource countdown;
    public AudioSource startGame;

    public AudioSource buttonClick;

    public AudioSource ambient;

    public AudioSource winSound;
    public AudioSource loseSound;
    public AudioSource overtime;

    public static SoundManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableAllSounds()
    {
        if (PlayerPrefs.GetInt("DisableSoundEffects") == 0){
            playerScore.mute = false;
            enemyScore.mute = false;

            puckCollide.mute = false;
            puckWallImpact.mute = false;

            joePowerup.mute = false;
            mummyPowerup.mute = false;
            vikingPowerUp.mute = false;
            yetiPowerup.mute = false;

            vikingShoot.mute = false;

            countdown.mute = false;
            startGame.mute = false;

            buttonClick.mute = false;
            ambient.mute = false;

            winSound.mute = false;
            loseSound.mute = false;
            overtime.mute = false;
        }
    }

    public void DisableAllSounds()
    {
        playerScore.mute = true;
        enemyScore.mute = true;

        puckCollide.mute = true;
        puckWallImpact.mute = true;

        joePowerup.mute = true;
        mummyPowerup.mute = true;
        vikingPowerUp.mute = true;
        yetiPowerup.mute = true;

        vikingShoot.mute = true;

        countdown.mute = true;
        startGame.mute = true;

        buttonClick.mute = true;

        ambient.mute = true;

        winSound.mute = true;
        loseSound.mute = true;
        overtime.mute = true;    
    }

    public void EnableMenuSounds()
    {
        DisableAllSounds();
        if (PlayerPrefs.GetInt("DisableSoundEffects") == 0)
        {
            buttonClick.mute = false;
        }
    }

    public void PlayPowerup(string type)
    {
        if (type == "Joe")
        {
            joePowerup.Play();
        }else if (type == "Mummy")
        {
            mummyPowerup.Play();
        }else if (type == "Viking")
        {
            vikingPowerUp.Play();
        }else if (type == "Yeti")
        {
            yetiPowerup.Play();
        }
    }

    public void ButtonClick()
    {
        //Too tedious to remove from every component
        //buttonClick.Play();
    }
}
