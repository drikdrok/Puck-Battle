using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Opponent : MonoBehaviour
{
    public Half area;

    public Transform selected;
    private List<Transform> shootAreas = new List<Transform>();
    public Transform shootAreaTarget;

    private Power power;


    float timer = 2.5f;


    private GameObject[] targets;


    public bool disabled = false;

    void Start()
    {
        power = GetComponent<Power>();
        targets = GameObject.FindGameObjectsWithTag("AimTarget");
        Debug.Log("Targets: " + targets.Length);

        //Find a shoot area
        foreach(Transform t in area.transform)
        {
            if (t.gameObject.CompareTag("ShootArea"))
            {
                shootAreas.Add(t.gameObject.transform);
            }
        }
        Debug.Log("Length of shoot areas: " + shootAreas.Count);
    }

    void Update()
    {
        if (disabled)
        {
            return;
        }

        if (area.pucks.Count > 0)
        {
            timer += Time.deltaTime;

        }

        if (timer > 2 && area.pucks.Count > 0)
        {
            int index = Random.Range(0, area.pucks.Count);
            selected = area.pucks[index];
            Debug.Log("areas: " + shootAreas.Count);
            shootAreaTarget = shootAreas[Random.Range(0, shootAreas.Count)];
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
