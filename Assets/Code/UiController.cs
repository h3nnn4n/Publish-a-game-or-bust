using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    Text textCreditsAmount;
    Text textLivesAmount;

    float credits;
    int nLives;

    void Start()
    {
        textCreditsAmount = GameObject.Find("TextCreditsAmount").GetComponent<Text>();
        textLivesAmount = GameObject.Find("TextLivesAmount").GetComponent<Text>();
    }

    public void SetLives(int nLives)
    {
        if (this.nLives != nLives)
        {
            this.nLives = nLives;
            textLivesAmount.text = this.nLives.ToString();
        }
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
