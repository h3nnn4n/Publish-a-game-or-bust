using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    Text textCreditsAmount;
    float credits;

    void Start()
    {
        textCreditsAmount = GameObject.Find("TextCreditsAmount").GetComponent<Text>();
    }

    void Update()
    {
        
    }

    public void SetCredits(float newCredits)
    {
        if (System.Math.Abs(newCredits - credits) > 1e-04)
        {
            credits = newCredits;
            textCreditsAmount.text = credits.ToString();
        }
    }
}
