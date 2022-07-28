using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SelectionBase]
public class Monster : MonoBehaviour
{
    [SerializeField] Sprite _deadSprite;
    [SerializeField] ParticleSystem _particleSystem;
    bool hasDied;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (ShouldDieFromCollision(collision))
        {
            StartCoroutine(Die());
        }
       
    }

    bool ShouldDieFromCollision(Collision2D collision)
    {
        if (hasDied)
            return false;
        RedBird bird = collision.gameObject.GetComponent<RedBird>();
        if (bird != null)
        {return true;}

        if (collision.contacts[0].normal.y < -0.5)
            return true;

        return false;
    }   

    IEnumerator Die()
    {
        hasDied = true;
        GetComponent<SpriteRenderer>().sprite = _deadSprite;
        _particleSystem.Play();
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
