using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Promo : MonoBehaviour
{

    public Animation[] anim;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (Animation a in anim)
            {
                a.Play();
            }
        }
    }
}
