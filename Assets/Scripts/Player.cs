using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    [SerializeField]
    private GameObject[] _models;

    [SerializeField]
    private GameObject _torchToInstatiate;

    private GameObject _torchOnGround = null;

    private GameManager _gameManager;

    [SerializeField]
    private float _speed = 4F;
    [SerializeField]
    private float _gravity = -10F;
    [SerializeField]
    private float _jumpForce = 8f;
    [SerializeField]
    private float _jumpHeight = 3f;
    [SerializeField]
    private float _fuelIncreaseOnPickup = 0.2f;

    private InputSystem_Actions _playerInput;
    private CharacterController _characterController;
    private InputAction _interactButton;
    private Vector2 _currentMovementInput;
    private Vector3 _currentMovement;
    private bool _isJumpPressed = false;
    private bool _isJumping = false;
    private bool _applyGravity = true;
    private bool _isNearTorch = false;
    private bool _isHoldingTorch = false;
    private float _initialJumpStartPos = 0;

    private void Awake()
    {
        foreach (GameObject model in _models)
        {
            if (model == null)
            {
                Debug.Log("Model is unnasigned, check Player prefab models");
            }
        }
        InitPlayerMovementInput();
        _characterController = gameObject.GetComponent<CharacterController>();
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
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
        UpdateTurnDirection();
        HandleTorchInteracitons();
    }

    private void HandleTorchInteracitons()
    {
        if (!_isNearTorch && _isHoldingTorch && _interactButton.WasPressedThisFrame())
        {
            _isHoldingTorch = false;
            _models[0].SetActive(true);
            _models[1].SetActive(false);
            Instantiate(_torchToInstatiate, transform.position, Quaternion.identity);
        }  

        if (_isNearTorch && !_isHoldingTorch && _interactButton.WasPressedThisFrame())
        {
            _isHoldingTorch = true;
            _models[0].SetActive(false);
            _models[1].SetActive(true);
            _isNearTorch = false;
            if (_torchOnGround != null)
            {
                Destroy(_torchOnGround);
                _torchOnGround = null;

            }
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
        _interactButton = _playerInput.FindAction("Interact");
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
        if (_isJumpPressed && _characterController.isGrounded && !_isJumping)
        {
            _applyGravity = false;
            _isJumping = true;
            _currentMovement.y = _jumpForce;
            _initialJumpStartPos = transform.position.y;
        }
        else if (!_isJumpPressed && _isJumping && _characterController.isGrounded)
        {
            _isJumping = false;
        }

        if (_isJumping && (transform.position.y - _initialJumpStartPos) >= _jumpHeight)
        {
            _applyGravity = true;
        }

    }

    private void UpdateTurnDirection()
    {
        if (_currentMovement.x != 0 || _currentMovement.z != 0)
        {
            Vector3 movementDirection = new Vector3(_currentMovementInput.x, 0, _currentMovementInput.y).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = lookRotation;
        }

    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "Crate" && !_isHoldingTorch)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            if (body != null && !body.isKinematic)
            {
                Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
	            body.AddForce(pushDirection * 10, ForceMode.Force);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Torch"))
        {
            _isNearTorch = true;
            _torchOnGround = other.gameObject;
        }
        if (other.CompareTag("Fuel") && _isHoldingTorch) {
            _gameManager.IncreaseFuel(_fuelIncreaseOnPickup);
            Destroy(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Torch"))
        {
            _isNearTorch = false;
        }
    }
}
