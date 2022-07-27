using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController_A : MonoBehaviour
{
    [SerializeField] string _nextLevelName;

    Dragon[] _dragons;

    void OnEnable()
    {
        _dragons = FindObjectsOfType<Dragon>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (MonstersAreAllDead())
       //     GoToNextLevel();
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
}
