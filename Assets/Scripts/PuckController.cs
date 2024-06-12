using PolygonArsenal;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public bool frozen = false;

    private bool invulnerable = false;


    [SerializeField] private GameObject tornadoPrefab;
    private Transform tornado;
    private GameObject repulse;

    public Half currentHalf;


    private float scaleFactor = 1;
    private Vector3 scaleVec;
    private int bounces = 0;

    [SerializeField] private GameObject firePrefab;
    private Transform fireFlame;

    public float timeSinceShot = 0;

    [SerializeField] private GameObject joeEffectPrefab;
    private Transform joeEffect;

    [SerializeField] private GameObject scoreEffectPrefab;


    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.maxAngularVelocity = 5;
        meshRenderer = GetComponent<MeshRenderer>();

        tornado = Instantiate(tornadoPrefab, transform.position, Quaternion.Euler(-90, 0, 0)).GetComponent<Transform>();
        tornado.localScale = Vector3.one * 2.5f;
        tornado.GetComponent<ParticleSystem>().Stop();

        fireFlame = Instantiate(firePrefab, transform.position, Quaternion.Euler(-90, 0, 0)).GetComponent<Transform>();
        fireFlame.GetComponent<ParticleSystem>().Stop();

        repulse = GetComponentInChildren<Repulse>().gameObject;
        repulse.GetComponent<Repulse>().parent = gameObject;
        repulse.SetActive(false);

        scaleVec = transform.localScale;

    }

    void Update()
    {
        tornado.position = transform.position;
        fireFlame.position = transform.position;
        if (joeEffect)
        {
            joeEffect.position = transform.position;
        }
        timeSinceShot += Time.deltaTime;
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
                    SoundManager.instance.playerScore.Play();
                }
            }
            else
            {
                if (enemyScore) { 
                    enemyScore.AddScore(1);
                    SoundManager.instance.enemyScore.Play();
                }
            }

            //Score goal, remove puck
            if (currentHalf)
            {
                currentHalf.pucks.Remove(transform);
            }


            if (respawnOnGoal)
            {
                PuckSpawner.instance.NewPuck();
            }

            Instantiate(scoreEffectPrefab, transform.position, Quaternion.Euler(-90, 0, 0));

            Destroy(tornado.gameObject);
            Destroy(fireFlame.gameObject);
            Destroy(gameObject);
        }
        else if (other.transform.CompareTag("AimTarget"))
        {
            //Make puck not get stuck
            //transform.position += Vector3.up * 0.3f;
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
            SoundManager.instance.puckCollide.Play();
            PuckController other = collision.gameObject.GetComponent<PuckController>();

            if (other.poweredUp != "none")
            {
                if (other.timeSinceShot < 0.6f )
                {
                    if (playerScore.gameObject.GetComponent<PlayerController>().puck == this) // This don't work
                    {
                        Debug.Log("Grip lost");
                        playerScore.gameObject.GetComponent<PlayerController>().LoseGrib();
                    }
                }
            }


            if (poweredUp == "Yeti")
            {
                if (shotBy == "Player" && !other.isInPlayerHalf)
                {
                    DePower(true);
                    if (other.poweredUp == "Yeti")
                    {
                        other.DePower(true);
                    }
                    else
                    {
                        other.StartCoroutine(other.Freeze());

                    }
                }
                else if (shotBy == "Enemy" && other.isInPlayerHalf)
                {
                    DePower(true);
                    if (other.poweredUp == "Yeti")
                    {
                        other.DePower(true);
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
            SoundManager.instance.puckWallImpact.Play();
            bounces++;
            DePower(false);
        }
    }

    public void SetPowered(string type)
    {
        if (poweredUp != "none")
        {
            return;
        }

        poweredUp = type;
        SoundManager.instance.PlayPowerup(type);
        if (type == "Yeti")
        {
            meshRenderer.material = poweredYetiMaterial;
        }else if (type == "Viking")
        {
            //meshRenderer.material = poweredMaterial;
            StartCoroutine(StartGrowing());
            fireFlame.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DepowerInSeconds(3f, true));
        }
        else if (type == "Mummy")
        {
            tornado.GetComponent<ParticleSystem>().Play();
            repulse.SetActive(true);
            StartCoroutine(DepowerInSeconds(3, true));
        }else if (type == "Joe")
        {
            meshRenderer.material = poweredMaterial;
            shotPower = 55;
            joeEffect = Instantiate(joeEffectPrefab, transform.position, Quaternion.Euler(-90, 0, 0)).transform;
        }
        else
        {
            meshRenderer.material = poweredMaterial;
        }
    }

    public void DePower(bool forced)
    {
        if (!forced)
        {
            if ((poweredUp == "Viking" || poweredUp == "Yeti") && bounces < 3) { return; }
        }

        if (poweredUp == "none") { return; }

        if (!frozen)
        {
            meshRenderer.material = puckMaterial;
            poweredUp = "none";
            shotPower = 40;

            //Viking
            StartCoroutine(StartShrinking());
            fireFlame.GetComponent<ParticleSystem>().Stop();

            //Mummy
            tornado.GetComponent<ParticleSystem>().Stop();
            repulse.SetActive(false);

            BecomeInvulnerable(0.1f);
        }
    }


    public void Shoot(Vector3 direction, string _shotBy)
    {
        rigidbody.velocity = direction * shotPower;
        shotBy = _shotBy;
        timeSinceShot = 0;

        if (poweredUp == "Yeti" || poweredUp == "Joe")
        {
            StartCoroutine(DepowerInSeconds(3, true));
        }

        if (poweredUp == "Viking")
        {
            SoundManager.instance.vikingShoot.Play();
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

    public IEnumerator DepowerInSeconds(float duration, bool forced)
    {
        yield return new WaitForSeconds(duration);
        DePower(forced);
    }

    public IEnumerator StartGrowing()
    {
        scaleFactor += Mathf.Lerp(1, 1.35f, scaleFactor) * Time.deltaTime;
        transform.localScale = scaleVec * scaleFactor;
        rigidbody.mass = 5;
        rigidbody.drag = 1;
        rigidbody.angularDrag = 0.5f;
        shotPower = 70;
        yield return 0; // Wait 1 frame
        if (scaleFactor <= 1.5f)
        {
            StartCoroutine(StartGrowing());
        }

    }

    public IEnumerator StartShrinking()
    {
        scaleFactor -= Mathf.Lerp(1, 1.5f, scaleFactor) * Time.deltaTime;
        transform.localScale = scaleVec * scaleFactor;
        rigidbody.mass = 1;
        rigidbody.drag = 0;
        rigidbody.angularDrag = 0.05f;
        shotPower = 40;
        yield return 0; // Wait 1 frame
        if (scaleFactor > 1f)
        {
            StartCoroutine(StartShrinking());
        }
    }
}
