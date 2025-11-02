using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float _speed = 4F;
    [SerializeField]
    private float _gravity = 10F;
    [SerializeField]
    private float _jumpForce = 2f;
    private InputSystem_Actions _playerInput;
    private CharacterController _characterController;
    private Vector2 _currentMovementInput;
    private Vector3 _currentMovement;
    private bool _isJumpPressed = false;
    private bool _applyGravity = true;

    private void Awake() {
        InitPlayerMovementInput();
        _characterController = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        _characterController.Move(_speed * Time.deltaTime * _currentMovement);
        if (_isJumpPressed)
        {
            _characterController.Move(new Vector3(_currentMovement.x, _jumpForce, _currentMovement.z) * Time.deltaTime);
            _applyGravity = false;
        } else
        {
            _applyGravity = true;
        }
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
        _currentMovement.x = _currentMovementInput.x;
        _currentMovement.z = _currentMovementInput.y;

    }
    private void OnJumpInput(InputAction.CallbackContext context)
    {
        _isJumpPressed = context.ReadValueAsButton();
    }
    private void ApplyGravity()
    {
        _characterController.Move(new Vector3(_currentMovement.x, -_gravity, _currentMovement.z) * Time.deltaTime);
    }
}
