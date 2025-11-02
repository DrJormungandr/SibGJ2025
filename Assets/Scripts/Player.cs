using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float _speed = 4F;
    [SerializeField]
    private float _gravity = -10F;
    [SerializeField]
    private float _jumpForce = 8f;
    [SerializeField]
    private float _jumpHeight = 3f;

    private InputSystem_Actions _playerInput;
    private CharacterController _characterController;
    private Vector2 _currentMovementInput;
    private Vector3 _currentMovement;
    private bool _isJumpPressed = false;
    private bool _isJumping = false;
    private bool _applyGravity = true;

    private void Awake()
    {
        InitPlayerMovementInput();
        _characterController = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        _characterController.Move(Time.deltaTime * _currentMovement);
        HandleJump();
        if (_applyGravity)
        {
            ApplyGravity();
        }
    }
    private void InitPlayerMovementInput()
    {
        _playerInput = new InputSystem_Actions();
        _playerInput.Player.Enable();
        _playerInput.Player.Move.started += OnMovementInput;
        _playerInput.Player.Move.canceled += OnMovementInput;
        _playerInput.Player.Move.performed += OnMovementInput;
        _playerInput.Player.Jump.started += OnJumpInput;
        _playerInput.Player.Jump.canceled += OnJumpInput;
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        _currentMovement.x = _currentMovementInput.x * _speed;
        _currentMovement.z = _currentMovementInput.y * _speed;

    }
    private void OnJumpInput(InputAction.CallbackContext context)
    {
        _isJumpPressed = context.ReadValueAsButton();
    }
    private void ApplyGravity()
    {
        _currentMovement.y = _gravity;
    }
    private void HandleJump()
    {
        float initialJumpStartPos = 0;
        if (_isJumpPressed && _characterController.isGrounded && !_isJumping)
        {
            _applyGravity = false;
            _isJumping = true;
            _currentMovement.y = _jumpForce;
            initialJumpStartPos = transform.position.y;
        }
        else if (!_isJumpPressed && _isJumping && _characterController.isGrounded)
        {
            _isJumping = false;
        }
                // Debug.Log(transform.position.y - initialJumpStartPos);

        if (_isJumping && (transform.position.y - initialJumpStartPos) >= _jumpHeight)
        {
            _applyGravity = true;
        }

    }
}
