using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Opponent : MonoBehaviour
{
    public Half area;

    public Transform selected;
    public Transform shootAreaTarget;

    private Power power;


    float timer = 2.5f;


    private GameObject[] targets;

    void Start()
    {
        power = GetComponent<Power>();
        targets = GameObject.FindGameObjectsWithTag("AimTarget");
        Debug.Log("Targets: " + targets.Length);

        //Find a shoot area
        foreach(Transform t in area.transform)
        {
            if (t.CompareTag("ShootArea"))
            {
                shootAreaTarget = t;
                break;
            }
        }
    }

    void Update()
    {

        if (area.pucks.Count > 0)
        {
            timer += Time.deltaTime;

        }

        if (timer > 2 && area.pucks.Count > 0)
        {
            int index = Random.Range(0, area.pucks.Count - 1);
            selected = area.pucks[index];
            timer = 0;
        }

        if (selected != null)
        {
            Rigidbody rb = selected.gameObject.GetComponent<Rigidbody>();
            PuckController puck = selected.GetComponent<PuckController>();
            if (power && power.GetValue() >= 1)
            {
                puck.SetPowered(StartManager.enemyName);
                power.SetValue(0);
            }
            selected.transform.position = Vector3.Lerp(selected.transform.position, shootAreaTarget.position, 3 * Time.deltaTime);
            
            if (timer > 0.7f) // Shoot
            {
                puck.transform.LookAt(targets[Random.Range(0, targets.Length)].transform);

                puck.Shoot(puck.transform.forward, "Enemy");

                selected = null;
                timer = Random.Range(-0.1f, 0.1f);
            }

        }


        
    }
}
