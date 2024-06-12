using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SineMovement : MonoBehaviour
{
    float timer = 0;

    public float speed = 0.0005f;
    public float moveDistance = 0.00005f;
    public float offset = 0;

    Vector3 originalPosition;


    private void Start()
    {
        originalPosition = transform.position;

        //Can't be bothered to manually change everything so we doing this lol
        if (gameObject.CompareTag("Goal"))
        {
            GetComponent<BoxCollider>().size = new Vector3(1, 10, 1);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        transform.position = originalPosition + new Vector3(Mathf.Sin(timer * speed + offset) * moveDistance, 0, 0);
    }
}
