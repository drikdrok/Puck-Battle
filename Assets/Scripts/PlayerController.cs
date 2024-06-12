using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Power power;

    public PuckController puck = null;

    public bool dragging = false;
    public bool shooting = false;
    public Vector3 shootStart;

    [SerializeField] private ShotIndicator shotIndicator;

    private Vector3 mousePos;

    [SerializeField] GameManager gameManager;



    void Start()
    {
        power = GetComponent<Power>();
    }

    void Update()
    {
        mousePos = GetMousePos();

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        LayerMask mask = LayerMask.GetMask("Puck");
        if (!shooting && !dragging)
        {
            if (Physics.Raycast(ray, out hit, 100, mask))
            {
                puck = hit.collider.gameObject.GetComponent<PuckController>();

                if (puck.shotBy == "Enemy" && puck.timeSinceShot <= 0.6f)
                {
                    puck = null;
                }
            }
            else
            {
                puck = null;
            }

        }


        if (puck != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                dragging = true;
                puck.GetComponent<PuckController>().shotBy = "Player";
                puck.GetComponent<Rigidbody>().velocity = Vector3.zero;

                if (gameManager.doingTutorial)
                {
                    gameManager.dragPuck();
                }
            }
            else if (puck.canShoot && Input.GetMouseButtonDown(1))
            {
                shooting = true;
                shootStart = GetMousePos();
            }

            Shoot();
            FollowMouse();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (power.GetValue() >= 1 && puck.poweredUp == "none")
                {
                    power.SetValue(0);
                    puck.SetPowered(StartManager.playerName);
                }
            }

            if (puck != null)
            {
                if (dragging && !puck.isInPlayerHalf)
                {
                    puck = null;
                    dragging = false;
                    shooting = false;
                    return;
                }
            }

        }
        if (Input.GetMouseButtonUp(0)) {
            if (puck)
            {
                //puck.gameObject.GetComponent<Rigidbody>().velocity *= 0.1f;
            }
            puck = null;
            dragging = false;
            shooting = false;
        }


        if (puck == null || !puck.canShoot)
        {
            shotIndicator.gameObject.SetActive(false);
        }

    }


    private Vector3 GetMousePos()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 0; // This might be a problem in the future
        if (puck)
        {
            mousePosition.z = (puck.transform.position - Camera.main.transform.position).magnitude;
        }
        Vector3 pos = Camera.main.ScreenToWorldPoint(mousePosition);
        pos.y = 1.05f;
        return pos;
    }


    private void FollowMouse()
    {
        if (dragging)
        {
            float boardWidth = 16.5f;
            //puck.gameObject.GetComponent<Rigidbody>().velocity = (mousePos - puck.transform.position) * 10f;
            Vector3 newPos = new Vector3(Mathf.Clamp(mousePos.x, -boardWidth, boardWidth), mousePos.y, Mathf.Clamp(mousePos.z, -20.5f, -1.5f));
            
            puck.gameObject.GetComponent<Transform>().position = Vector3.Lerp(puck.gameObject.GetComponent<Transform>().position, newPos, 14 * Time.deltaTime);
        }
    }

    private void Shoot()
    {
        if (shooting)
        {
            //Calculate shot power
            Vector3 shotLoad = (shootStart - GetMousePos()) / 2;
            shotLoad.y = 0;
            if (shotLoad.magnitude > 1)
            {
                shotLoad = shotLoad.normalized;
            }

            shotIndicator.puck = puck.transform;
            shotIndicator.SetPower(shotLoad.magnitude);
            shotIndicator.SetAngle(Mathf.Atan2(puck.transform.position.z - mousePos.z, puck.transform.position.x - mousePos.x));
            shotIndicator.gameObject.SetActive(true);


            if (Input.GetMouseButtonUp(1) && puck)
            {
                puck.Shoot(shotLoad, "Player");

                shooting = false;

                puck = null;

                gameManager.shootPuck();
              
            }
        }
    }

    public void LoseGrib()
    {
        if (puck)
        {
            puck = null;
        }
    }

}
