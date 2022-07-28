using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBird : MonoBehaviour
{
    //[SerializeField] float _launchForce = 500;
    //[SerializeField] float _maxDragDistance = 5;
    [SerializeField] Sprite _deadSprite;
    [SerializeField] float _flySpeed = 4f;
    [SerializeField] float viewRange = 2;
    [SerializeField] bool limitCameraView;
    Transform upperFocus, lowerFocus, useWhenLow, useWhenHigh;
    float constant_x_position1;
    float constant_x_position2;
    GameObject targetGroupWhenNormal;
    GameObject targetGroupWhenLow;
    GameObject targetGroupWhenHigh;
    Cinemachine.CinemachineVirtualCamera cm_camera;

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
    float _upperScreenBound;//4.6f;
    float _lowerScreenBound;//-2.6f;
    
    //new Cinemachine.CinemachineTargetGroup camera;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _state = State.IdleDescend;
        upperFocus = this.gameObject.transform.Find("upperFocus");
        lowerFocus = this.gameObject.transform.Find("lowerFocus");
        useWhenHigh = GameObject.Find("raiseHigher").transform;
        useWhenLow = GameObject.Find("pullDown").transform;
        constant_x_position1 = upperFocus.position.x;
        constant_x_position2 = useWhenHigh.position.x;
        _upperScreenBound = GameObject.Find("Mountains & Clouds").transform.position.y + (float)6.8;
        _lowerScreenBound= GameObject.Find("Ground").transform.position.y + (float)1.45;

        cm_camera = GameObject.Find("CM vcam1").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        targetGroupWhenHigh = GameObject.Find("TargetGroupWhenHigh");
        targetGroupWhenLow = GameObject.Find("TargetGroupWhenLow"); ;
        targetGroupWhenNormal = GameObject.Find("TargetGroup");
    }

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = _rigidbody2D.position;
        _rigidbody2D.isKinematic = true;

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

    void keepFlying()
    {
        if (Input.GetKeyDown(KeyCode.Space))
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

        if (_state == State.Ascend)
        {
            FlyUp();
        }
        else if (_state == State.Descend)
        {
            FallDown();
        }
        else
        {
            StopMoving();
        }

        // Keeping the position of the focus objects relative to the bird's y position ONLY.
        upperFocus.position = new Vector2(constant_x_position1, this.transform.position.y + viewRange);
        lowerFocus.position = new Vector2(constant_x_position1, this.transform.position.y - viewRange);
        useWhenHigh.position = new Vector2(constant_x_position2, this.transform.position.y + (viewRange+(float)3.6));
        useWhenLow.position = new Vector2(constant_x_position2, this.transform.position.y - (viewRange+(float)3.6));
        if (limitCameraView)
        {
            float player_height = gameObject.transform.position.y;
            if (player_height <= -1.6)  // if i'm too low, lift the camera view
            {
                cm_camera.Follow = targetGroupWhenLow.transform;
            }
            else if (player_height >= 6.14)  // if i'm too high, lower the camera view
            {
                cm_camera.Follow = targetGroupWhenHigh.transform;
            }
            else // i'm at the middle of the playground. keep the camera centered.
            {
                if (_state != State.Ascend && _state != State.Descend)
                    cm_camera.Follow = targetGroupWhenNormal.transform;
            }
            if (_state == State.Ascend || _state == State.Descend)
                cm_camera.Follow = targetGroupWhenNormal.transform;
        }else cm_camera.Follow = targetGroupWhenNormal.transform;
    }

    // Update is called once per frame
    void Update()
    {
        keepFlying();
        
    }


    //void OnMouseDown()
    //{
    //_spriteRenderer.color = Color.red;    
    //}

    /*
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
    */

    /*  MAYBE WE NEED THIS
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
    */
}
