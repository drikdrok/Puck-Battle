using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
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

    public bool respawnOnGoal = true;
    public int spawnHalf = -1;

    public bool isInPlayerHalf = false;
    public string shotBy = "none";

    private ScoreHandler playerScore;
    private ScoreHandler enemyScore;


    private MeshRenderer meshRenderer;

    private bool frozen = false;

    private bool invulnerable = false;


    private Transform vikingPuck1;
    private Transform vikingPuck2;


    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
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

            if (respawnOnGoal)
            {
                PuckSpawner.instance.NewPuck();
            }
            
            Destroy(gameObject);
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
                    DePower();
                    if (other.poweredUp == "Yeti")
                    {
                        other.DePower();
                    }
                    else
                    {
                        other.StartCoroutine(other.Freeze());

                    }
                }
                else if (shotBy == "Enemy" && other.isInPlayerHalf)
                {
                    DePower();
                    if (other.poweredUp == "Yeti")
                    {
                        other.DePower();
                    }
                    else
                    {
                        other.StartCoroutine(other.Freeze());
                    }
                }
                
            }

        }
        else if (!collision.gameObject.CompareTag("Ground"))
        {
            DePower();
        }
    }

    public void SetPowered(string type)
    {
        if (poweredUp != "none")
        {
            return;
        }

        poweredUp = type;
        if (type == "Yeti")
        {
            meshRenderer.material = poweredYetiMaterial;
        }else if (type == "Viking")
        {
            meshRenderer.material = poweredMaterial;
            vikingPuck1 = PuckSpawner.instance.NewVikingPuck(transform.position + transform.right * 2);
            vikingPuck2 = PuckSpawner.instance.NewVikingPuck(transform.position - transform.right * 2);
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

            vikingPuck1 = null;
            vikingPuck2 = null;

            BecomeInvulnerable(0.1f);
        }
    }


    public void Shoot(Vector3 direction, string _shotBy)
    {
        rigidbody.velocity = direction * shotPower;
        shotBy = _shotBy;

        if (poweredUp == "Yeti")
        {
            DepowerInSeconds(3);
        }else if (poweredUp == "Viking")
        {
            DePower();
            vikingPuck1.GetComponent<Rigidbody>().velocity = rigidbody.velocity;
            vikingPuck2.GetComponent<Rigidbody>().velocity = rigidbody.velocity;
        }
    }


    public IEnumerator Freeze()
    {
        if (poweredUp != "none" || invulnerable == true)
        {
            Debug.Log("Should happen");
            yield break;
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

    public IEnumerator BecomeInvulnerable(float duration)
    {
        invulnerable = true;

        yield return new WaitForSeconds(duration);

        invulnerable = false;
    }

    public IEnumerator DepowerInSeconds(float duration)
    {
        yield return new WaitForSeconds(duration);
        DePower();
    }
}
