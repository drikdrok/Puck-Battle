using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{

    public int score = 0;

    [SerializeField] private TextMeshProUGUI text;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void AddScore(int amount)
    {
        if (text != null)
        {
            score += amount;
            text.text = score.ToString();
        }
    }
}
