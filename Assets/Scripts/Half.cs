using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Half : MonoBehaviour
{
    public List<Transform> pucks;

    public bool isPlayerHalf = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Puck"))
        {
            pucks.Add(other.transform);
            other.GetComponent<PuckController>().isInPlayerHalf = isPlayerHalf;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Puck"))
        {
            pucks.Remove(other.transform);
            other.GetComponent<PuckController>().isInPlayerHalf = !isPlayerHalf;

        }
    }
}
