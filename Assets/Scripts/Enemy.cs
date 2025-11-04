using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1;
    [SerializeField]
    private float _travelDistance = 3;
    [SerializeField]
    private MovementAxis _movementAxis = MovementAxis.Vertical;
    private Vector3 _initialPosition;
    private Vector3 _movementVector;
    private bool _movingBack = false;
    

    void Start()
    {
        _initialPosition = transform.position;
        _movementVector = new Vector3(0, 0, 1 * _speed);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (_movementAxis == MovementAxis.Vertical)
        {
            VerticalMovement();
        } else
        {
            HorizontalMovement();
        }

    }

    private void VerticalMovement()
    {
        transform.Translate(_movementVector * Time.deltaTime);
        if (!_movingBack)
        {
            if (Math.Abs(transform.position.z - _initialPosition.z) > _travelDistance)
            {
                ChangeDirection();
            }
        }
        else
        {
            if (Math.Abs(_initialPosition.z - transform.position.z) > _travelDistance)
            {
                ChangeDirection();
            }
        }
    }

    private void HorizontalMovement()
    {
        transform.Translate(_movementVector * Time.deltaTime);
        if (!_movingBack)
        {
            if (Math.Abs(transform.position.x - _initialPosition.x) > _travelDistance)
            {
                ChangeDirection();
            }
        }
        else
        {
            if (Math.Abs(_initialPosition.x - transform.position.x) > _travelDistance)
            {
                ChangeDirection();
            }
        }
    }
    
    private void ChangeDirection()
    {
        _movementVector *= -1;
    }
}
    
