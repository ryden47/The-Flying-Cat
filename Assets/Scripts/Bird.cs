using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] float _launchForce = 500;
    [SerializeField] float _maxDragDistance = 5;
    [SerializeField] float _flySpeed = 4f;

    enum State
    {
        IdleAscend,
        IdleDescend,
        Ascend,
        Descend
    }

    Vector2 _startPosition;
    Rigidbody2D _rigidbody2D;
    SpriteRenderer _spriteRenderer;
    State _state;
    float _upperScreenBound = 4.6f;
    float _lowerScreenBound = -2.6f;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _state = State.IdleDescend;
    }
    // Start is called before the first frame update
    void Start()
    {
        _startPosition = _rigidbody2D.position;
        _rigidbody2D.isKinematic = true;
    }

    void OnMouseDown()
    {
        _spriteRenderer.color = Color.red;    
    }

     void OnMouseUp()
    {
        Vector2 currentPosition = _rigidbody2D.position;
        Vector2 direction = _startPosition - currentPosition;
        direction.Normalize();

        _rigidbody2D.isKinematic = false;
        _rigidbody2D.AddForce(direction * _launchForce);

        _spriteRenderer.color = Color.white;

    }

    void OnMouseDrag()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 desiredPosition = mousePosition;
      
        float distance = Vector2.Distance(desiredPosition, _startPosition);
        if(distance > _maxDragDistance)
        {
            Vector2 direction = desiredPosition - _startPosition;
            direction.Normalize();
            desiredPosition = _startPosition + (direction * _maxDragDistance);
        }

        if (desiredPosition.x > _startPosition.x)
            desiredPosition.x = _startPosition.x;

        _rigidbody2D.position = desiredPosition;
    }

    void FlyUp()
    {
        if (transform.position.y < _upperScreenBound)
        {
            transform.Translate(Vector2.up * _flySpeed * Time.deltaTime, Space.World);
        }
        else
        {
            _state = State.IdleAscend;
            transform.Rotate(0.0f, 0.0f, -30f, Space.Self);
        }
    }

    void FallDown()
    {
        if (transform.position.y > _lowerScreenBound)
        {
            transform.Translate(Vector2.down * _flySpeed * Time.deltaTime, Space.World);
        }
        else
        {
            _state = State.IdleDescend;
            transform.Rotate(0.0f, 0.0f, 30f, Space.Self);
        }
    }

    void StopMoving()
    {
        _rigidbody2D.velocity = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            switch (_state)
            {
                case State.IdleDescend:
                    transform.Rotate(0.0f, 0.0f, 30f, Space.Self);
                    _state = State.Ascend;
                    break;
                case State.Ascend:
                    transform.Rotate(0.0f, 0.0f, -30f, Space.Self);
                    _state = State.IdleAscend;
                    break;
                case State.IdleAscend:
                    transform.Rotate(0.0f, 0.0f, -30f, Space.Self);
                    _state = State.Descend;
                    break;
                case State.Descend:
                    transform.Rotate(0.0f, 0.0f, 30f, Space.Self);
                    _state = State.IdleDescend;
                    break;
            }
        }

        if(_state == State.Ascend)
        {
            FlyUp();
        }
        else if(_state == State.Descend)
        {
            FallDown();
        }
        else
        {
            StopMoving();
        }
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(ResetAfterDelay());
    }

    IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(3);
        _rigidbody2D.position = _startPosition;
        _rigidbody2D.isKinematic = true;
        _rigidbody2D.velocity = Vector2.zero;
    }
}
