using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{

    //[SerializeField] public float speed = (float)-3d;  // maybe change to float?
    private float speed;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    private LevelController_A levelController;
    private float width;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        levelController = GameObject.Find("LevelController").GetComponent<LevelController_A>();

        speed = 1;
        width = boxCollider.size.x;
        boxCollider.enabled = false;
        rb.velocity = new Vector2(-speed, 0);
        
    }

    // Update is called once per frame
    void Update()
    {
        speed = levelController.backgroundSpeed; // updating speed
        rb.velocity = new Vector2(-speed, 0);
        if (transform.position.x < -width)
        {
            Reposition();
        }
    }

    private void Reposition()
    {
        Vector2 vector = new Vector2(width * 2f, 0);
        transform.position = (Vector2)transform.position + vector;
    }
}
