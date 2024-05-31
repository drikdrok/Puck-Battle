using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PuckController : MonoBehaviour
{
    private Rigidbody rigidbody;

    public bool canShoot = false;



    private bool poweredUp = false;

    [SerializeField] private Material puckMaterial;
    [SerializeField] private Material poweredMaterial;

    private Vector3 spawnPoint;

    public bool isInPlayerHalf = false;

    private ScoreHandler playerScore;
    private ScoreHandler enemyScore;


    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        spawnPoint = transform.position;
    }

    void Update()
    {
        //For some reason this can't go in start
        if (!playerScore)
        {
            GameObject p = GameObject.FindWithTag("Player");
            if (p != null )
            {
                playerScore = p.GetComponent<ScoreHandler>();

            }
        }
        if (!enemyScore)
        {
            GameObject e = GameObject.FindWithTag("Enemy");
            if (e != null)
            {
                enemyScore = e.GetComponent<ScoreHandler>();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("ShootArea"))
        {
            canShoot = true;
        }else if (other.transform.CompareTag("Goal"))
        {
            if (!isInPlayerHalf)
            {
                playerScore.AddScore(1);
            }
            else
            {
                enemyScore.AddScore(1);
            }



            transform.position = spawnPoint;
            rigidbody.velocity = Vector3.zero;
            poweredUp = false;
        }
        else if (other.transform.CompareTag("AimTarget"))
        {
            //Make puck not get stuck
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.6f, transform.position.z);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("ShootArea"))
        {
            canShoot = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ground"))
        {
            SetPowered(false);
        }
    }

    public void SetPowered(bool enabled)
    {
        if (enabled)
        {
            GetComponent<MeshRenderer>().material = poweredMaterial;
        }
        else
        {
            GetComponent<MeshRenderer>().material = puckMaterial;
        }
    }

    public bool isPowered()
    {
        return poweredUp;
    }

}
