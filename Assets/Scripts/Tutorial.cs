using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Tutorial : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI text;
    [SerializeField] RectTransform panel;
    RectTransform position;

    private int stage = 0;

    private float scaleFactor;
    void Start()
    {
        scaleFactor = 800 / Screen.width;
        Debug.Log("Scale: " + scaleFactor);
        position = GetComponent<RectTransform>();
        //position.position = new Vector3(550, 200, 0);
        gameObject.SetActive(false);

        text.text = "Drag Pucks with Left Mouse Button!";
        panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 60);
    }

    void Update()
    {
        
    }


    public void dragPuck()
    {
        if (stage == 0)
        {
            stage = 1;
            text.text = "When a Puck is in a Red Shooting Area, Use Right Mouse Button to Charge up a Shot!";
           // position.position = new Vector3(50 , 135, 0);;
            panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 110);
            Time.timeScale = 1;

        }
    }

    public void Power()
    {
        if (PlayerPrefs.GetInt("DoTutorial") == 1 && stage == 3)
        {
            gameObject.SetActive(false);
            PlayerPrefs.SetInt("DoTutorial", 0);
        }
    }

    public void shootPuck()
    {
        if (stage == 1)
        {
            stage = 2;
            text.text = "Shoot Pucks Into Blue Goal to Score!";
            panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 65);

        }
        else if (stage == 2)
        {
            stage = 3;
            text.text = "When the meter is full, press Space to power up your puck!";
            panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 85);
            //gameObject.SetActive(false);
            //  PlayerPrefs.SetInt("DoTutorial", 0);
        }
    }
}
