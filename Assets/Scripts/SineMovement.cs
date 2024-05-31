using System.Collections;
using System.Collections.Generic;
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
    }

    void Update()
    {
        timer += Time.deltaTime;
        transform.position = originalPosition + new Vector3(Mathf.Sin(timer * speed + offset) * moveDistance, 0, 0);
    }
}
