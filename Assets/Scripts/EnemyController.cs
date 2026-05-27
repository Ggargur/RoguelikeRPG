using System;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D), typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour, IMoveAction
{
    public event Action<Vector2> OnMoved;
    public float speed = 3f;

    private Rigidbody2D _rb;
    private Transform _playerTransform;
    private Vector2 _direction;
    private Vector2 _lastDirection;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_playerTransform == null) return;
        
        Vector2 directionToPlayer = (_playerTransform.position - transform.position).normalized;
        
        if (directionToPlayer != _lastDirection)
        {
            _lastDirection = directionToPlayer;
            _direction = directionToPlayer;
            OnMoved?.Invoke(_direction);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.IsPlayer())
        {
            _playerTransform = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.IsPlayer())
        {
            _playerTransform = null;
            _direction = Vector2.zero;
            _lastDirection = Vector2.zero;
            OnMoved?.Invoke(_direction);
        }
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = _direction * speed;
    }
}





