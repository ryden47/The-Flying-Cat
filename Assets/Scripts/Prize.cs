using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prize : MonoBehaviour
{
    [SerializeField] ParticleSystem _particleSystem;
    [SerializeField] float speed = 5;

    public int coinValue;
    private Rigidbody2D rb;
    private float leftBorder;
    public static int totalCoins = 0;


    // Use this for initialization
    void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
        //gameLevelManager = FindObjectOfType<LevelManager>();

        rb = this.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(-speed, 0);

        var dist = (transform.position - Camera.main.transform.position).z;
        leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;

        GameObject ground = GameObject.FindGameObjectWithTag("Background");
        Physics2D.IgnoreCollision(ground.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        RedBird player = other.gameObject.GetComponent<RedBird>();
        if (player != null)
        {
            //gameLevelManager.AddCoins(coinValue);
            totalCoins+=coinValue;
            StartCoroutine(CollectPrize());

        }
    }

    IEnumerator CollectPrize()
    {
        _particleSystem.Play();
        yield return new WaitForSeconds((float)0.25);
        gameObject.SetActive(false);
    }
}
