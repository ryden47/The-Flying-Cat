using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{

    public TMPro.TextMeshProUGUI pointsText;
    public void Setup(int score)
    {
        gameObject.SetActive(true);
        pointsText.text = score.ToString() + " COINS";
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("Level1.0");
    }

    public void ExitButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
