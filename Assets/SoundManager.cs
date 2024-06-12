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

    public static SoundManager instance;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
        }else if (type == "yeti")
        {
            yetiPowerup.Play();
        }
    }
}
