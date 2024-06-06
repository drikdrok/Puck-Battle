using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Repulse : MonoBehaviour
{

    private float repulseForce = 650;
    private float repulseRadius = 5.5f;
    public GameObject parent;
    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, repulseRadius);

        // Iterate through each collider
        foreach (Collider col in colliders)
        {
            if (col.gameObject == parent) { continue; }
            if (!col.gameObject.CompareTag("Puck")) { continue; }

            Rigidbody rb = col.GetComponent<Rigidbody>();
            rb.AddExplosionForce(repulseForce, transform.position, repulseRadius);
        }
    }
}
