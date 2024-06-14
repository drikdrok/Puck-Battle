using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;


public class ShotIndicator : MonoBehaviour
{

    [SerializeField] private Transform s1;
    [SerializeField] private Transform s2;
    [SerializeField] private Transform s3;

    public float power = 0;
    public float angle = 0;

    public Transform puck;
    public Vector3 mousePos;

    void Start()
    {
        
    }

    void LateUpdate()
    {
        if (!puck) return;


        Vector3 direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
        Vector3 startPos = puck.position + direction * 1.5f;

        s1.position = startPos;
        s2.position = startPos;
        s3.position = startPos;

        if (power > 0.2)
        {
            s2.position = startPos + direction * (Mathf.Clamp((power - 0.2f) / 0.4f, 0, 1));
        }
        if (power > 0.6)
        {
            s3.position = startPos + direction * 2 * (Mathf.Clamp((power - 0.6f) / 0.4f, 0, 1));
        }

        puck.LookAt(s2);
    }

    public void SetAngle(float angle)
    {
        this.angle = angle;
    }

    public void SetPower(float power)
    {
        this.power = power;
    }
}
