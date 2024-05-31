using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class Power : MonoBehaviour
{
    public Slider slider;
    public Image fill;

    public float speed = 3;


    float value = 0;

    void Update()
    {
        AddValue(Time.deltaTime / speed);
    }

    public void AddValue(float amount)
    {
        value += amount;
        slider.value = value;

        if (value >= 1)
        {
            value = 1;
            fill.color = Color.green;
        }
        else
        {
            fill.color = Color.red;
        }
    }

    public void SetValue(float _value)
    {
        value = _value;
        slider.value = value;

        if (value >= 1)
        {
            value = 1;
            fill.color = Color.green;
        }
    }

    public float GetValue()
    {
        return value;
    }
}
