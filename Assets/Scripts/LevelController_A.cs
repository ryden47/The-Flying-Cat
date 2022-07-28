using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController_A : MonoBehaviour
{
    [SerializeField] string _nextLevelName;
    [SerializeField] public float backgroundSpeed;
    [SerializeField] public bool godMode = false;

    RedBird player;
    CoinCounter coinCounter;
    public GameOverScreen gameOverScreen;
    Dragon[] _dragons;

    void OnEnable()
    {
        _dragons = FindObjectsOfType<Dragon>();
        player = GameObject.Find("Red Bird").GetComponent<RedBird>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (MonstersAreAllDead())
        //    GoToNextLevel();
        if (player.falling[2] == false)
            gameOverScreen.Setup(Prize.totalCoins);
    }

    private void GoToNextLevel()
    {
        Debug.Log("Go To Level" + _nextLevelName);
        SceneManager.LoadScene(_nextLevelName);
    }

    private bool MonstersAreAllDead()
    {
        foreach (var dragon in _dragons) {
            if (dragon.gameObject.activeSelf)
                return false;
        }

        return true;
    }

    public void GameOver()
    {
        if (!godMode)
        {
            player.Die();
            // wait until cat falls!
        }
        else
        {
            Debug.Log("Can't die when in god mod");
        }

    }
}
