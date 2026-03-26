using System;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IMoveAction
{
    public event Action<Vector2> OnMoved;
}

public class MovementController : MonoBehaviour, IMoveAction
{
    public event Action<Vector2> OnMoved;
    public float speed = 5f;

    [Header("Input")]
    [SerializeField] private InputActionReference moveAction;

    private Rigidbody2D _rb;
    private Vector2 _direction;
    private Vector2 _lastDirection;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        moveAction.action.performed += OnMove;
        moveAction.action.canceled += OnMove;
    }

    private void OnDisable()
    {
        moveAction.action.performed -= OnMove;
        moveAction.action.canceled -= OnMove;
        moveAction.action.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _direction = context.ReadValue<Vector2>().normalized;

        if (_direction == _lastDirection) return;
        
        _lastDirection = _direction;
        OnMoved?.Invoke(_direction);
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = _direction * speed;
    }
}