using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SC_CoinCounter : MonoBehaviour
{
    TextMeshProUGUI counterText;

    // Start is called before the first frame update
    void Start()
    {
        counterText = GetComponent<TextMeshProUGUI>();
        counterText.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        //Set the current number of coins to display
        if (counterText.text != Prize.totalCoins.ToString())
        {
            counterText.text = Prize.totalCoins.ToString();
        }
    }
}