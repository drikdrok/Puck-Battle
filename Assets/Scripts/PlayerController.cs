using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Power power;

    PuckController puck = null;

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
                if (power.GetValue() >= 1)
                {
                    power.SetValue(0);
                    puck.SetPowered(StartManager.playerName);
                }
            }

            if (puck != null)
            {
                if (!puck.isInPlayerHalf)
                {
                    puck.gameObject.GetComponent<Rigidbody>().velocity *= 0.05f;
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
                puck.gameObject.GetComponent<Rigidbody>().velocity *= 0.1f;
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
        pos.y = 1;
        return pos;
    }


    private void FollowMouse()
    {
        if (dragging)
        {
            puck.gameObject.GetComponent<Rigidbody>().velocity = (mousePos - puck.transform.position) * 10f;
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


            if (Input.GetMouseButtonUp(1))
            {
                puck.Shoot(shotLoad, "Player");

                shooting = false;

                puck = null;

                gameManager.shootPuck();
              
            }
        }
    }

}
