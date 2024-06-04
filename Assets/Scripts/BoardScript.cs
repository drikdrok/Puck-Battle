using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class BoardScript : MonoBehaviour
{


    [SerializeField] Transform[] playerJoeHalves;
    [SerializeField] Transform[] playerMummyHalves;
    [SerializeField] Transform[] playerVikingHalves;
    [SerializeField] Transform[] playerYetiHalves;
    
    [SerializeField] Transform[] enemyJoeHalves;
    [SerializeField] Transform[] enemyMummyHalves;
    [SerializeField] Transform[] enemyVikingHalves;
    [SerializeField] Transform[] enemyYetiHalves;

    [SerializeField] Opponent enemy;



    void Awake()
    {
        disableAll();

        Transform playerHalf = playerJoeHalves[Random.Range(0, playerJoeHalves.Length)];
        if (StartManager.playerName == "Joe")
        {
            playerHalf = playerJoeHalves[Random.Range(0, playerJoeHalves.Length)];
            playerHalf.gameObject.SetActive(true);
            playerHalf.position = new Vector3(0, 0, 0);
        }else if (StartManager.playerName == "Mummy")
        {
            playerHalf = playerMummyHalves[Random.Range(0, playerMummyHalves.Length)];
            playerHalf.gameObject.SetActive(true);
            playerHalf.position = new Vector3(0, 0, 0);
        } else if (StartManager.playerName == "Viking")
        {
            playerHalf = playerVikingHalves[Random.Range(0, playerVikingHalves.Length)];
            playerHalf.gameObject.SetActive(true);
            playerHalf.position = new Vector3(0, 0, 0);
        }else if (StartManager.playerName == "Yeti")
        {
            playerHalf = playerYetiHalves[Random.Range(0, playerYetiHalves.Length - 1)];
            playerHalf.gameObject.SetActive(true);
            playerHalf.position = new Vector3(0, 0, 0);
        }

        Transform enemyHalf = enemyJoeHalves[Random.Range(0, enemyJoeHalves.Length)];

        if (StartManager.enemyName == "Joe")
        {
            enemyHalf = enemyJoeHalves[Random.Range(0, enemyJoeHalves.Length)];
            enemyHalf.gameObject.SetActive(true);
            enemyHalf.position = new Vector3(0, 0, 0);

        } else if (StartManager.enemyName == "Mummy")
        {
            enemyHalf = enemyMummyHalves[Random.Range(0, enemyMummyHalves.Length)];
            enemyHalf.gameObject.SetActive(true);
            enemyHalf.position = new Vector3(0, 0, 0);
        }
        else if (StartManager.enemyName == "Viking")
        {
            enemyHalf = enemyVikingHalves[Random.Range(0, enemyVikingHalves.Length)];
            enemyHalf.gameObject.SetActive(true);
            enemyHalf.position = new Vector3(0, 0, 0);
        }
        else if (StartManager.enemyName == "Yeti")
        {
            enemyHalf = enemyYetiHalves[Random.Range(0, enemyYetiHalves.Length)];
            enemyHalf.gameObject.SetActive(true);
            enemyHalf.position = new Vector3(0, 0, 0);
        }

        enemy.area = enemyHalf.GetComponent<Half>();

        PuckSpawner.instance.playerHalf = playerHalf.GetComponent<Half>();
        PuckSpawner.instance.enemyHalf = enemyHalf.GetComponent<Half>();
    }

    private void disableAll()
    {
        for (int i = 0; i < playerJoeHalves.Length; i++)
        {
            playerJoeHalves[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < playerMummyHalves.Length; i++)
        {
            playerMummyHalves[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < playerVikingHalves.Length; i++)
        {
            playerVikingHalves[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < playerYetiHalves.Length; i++)
        {
            playerYetiHalves[i].gameObject.SetActive(false);
        }


        for (int i = 0; i < enemyJoeHalves.Length; i++)
        {
            enemyJoeHalves[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < enemyMummyHalves.Length; i++)
        {
            enemyMummyHalves[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < enemyVikingHalves.Length; i++)
        {
            enemyVikingHalves[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < enemyYetiHalves.Length; i++)
        {
            enemyYetiHalves[i].gameObject.SetActive(false);
        }


    }
}
