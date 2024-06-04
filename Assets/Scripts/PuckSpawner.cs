using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PuckSpawner : MonoBehaviour
{
    [SerializeField] GameObject PuckPrefab;

    public Half playerHalf;
    public Half enemyHalf;

    private float timer = 0;
    public float newPuckThreshold = 6;


    public static PuckSpawner instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= newPuckThreshold)
        {
            if (playerHalf.pucks.Count < 2 || enemyHalf.pucks.Count < 2)
            {
                NewPuck();
            }
            else
            {
                timer = 0;
            }
        }
    }

    public void NewPuck()
    {
        timer = 0;
        Vector3 spawn;
        if (playerHalf.pucks.Count >= enemyHalf.pucks.Count)
        {
            spawn = new Vector3(Random.Range(-12, 12), 8, Random.Range(2, 13));
        }
        else
        {
            spawn = new Vector3(Random.Range(-12, 12), 8, Random.Range(-2, -13));
        }
        
        Instantiate(PuckPrefab, spawn, Quaternion.identity);
    }

    public Transform NewVikingPuck(Vector3 position)
    {
        GameObject p = Instantiate(PuckPrefab, position, Quaternion.identity);
        p.GetComponent<PuckController>().respawnOnGoal = false;

        return p.GetComponent<Transform>();
    }
}
