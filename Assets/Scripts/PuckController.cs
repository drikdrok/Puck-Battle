using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PuckController : MonoBehaviour
{
    private Rigidbody rigidbody;

    public bool canShoot = false;

    public float shotPower = 40;

    public string poweredUp = "none";


    [SerializeField] private Material puckMaterial;
    [SerializeField] private Material poweredMaterial;
    [SerializeField] private Material poweredYetiMaterial;
    [SerializeField] private Material frozenMaterial;

    private Vector3 spawnPoint;

    public bool isInPlayerHalf = false;
    public string shotBy = "none";

    private ScoreHandler playerScore;
    private ScoreHandler enemyScore;


    private MeshRenderer meshRenderer;

    private bool frozen = false;


    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
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
                if (playerScore)
                {
                    playerScore.AddScore(1);
                }
            }
            else
            {
                if (enemyScore) { 
                    enemyScore.AddScore(1);
                }
            }



            transform.position = spawnPoint;
            rigidbody.velocity = Vector3.zero;
            DePower();
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
        if (collision.gameObject.CompareTag("Puck"))
        {

            if (poweredUp == "Yeti")
            {
                PuckController other = collision.gameObject.GetComponent<PuckController>();
                if (shotBy == "Player" && !other.isInPlayerHalf)
                {
                    other.StartCoroutine(other.Freeze());
                }else if (shotBy == "Enemy" && other.isInPlayerHalf)
                {
                    other.StartCoroutine(other.Freeze());
                }
            }

            DePower();
        }
        else if (!collision.gameObject.CompareTag("Ground"))
        {
            DePower();
        }
    }

    public void SetPowered(string type)
    {
        poweredUp = type;
        if (type == "Yeti")
        {
            meshRenderer.material = poweredYetiMaterial;
        }
        else
        {
            meshRenderer.material = poweredMaterial;
        }
    }

    public void DePower()
    {
        if (!frozen)
        {
            meshRenderer.material = puckMaterial;
            poweredUp = "none";
        }
    }


    public void Shoot(Vector3 direction, string _shotBy)
    {
        rigidbody.velocity = direction * shotPower;
        shotBy = _shotBy;
    }


    public IEnumerator Freeze()
    {
        if (poweredUp != "none")
        {
            yield return null;
        }

        rigidbody.velocity = Vector3.zero;
        rigidbody.isKinematic = true;
        frozen = true;
        meshRenderer.material = frozenMaterial;

        yield return new WaitForSeconds(5);

        rigidbody.isKinematic = false;
        frozen = false;
        meshRenderer.material = puckMaterial;
    }

}
