using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 0.8f;

    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private Transform cameraTransform;
    private Animator animator;

    private InputAction moveAction;
    private InputAction jumpAction;



    private void Start()
    {
        // Initialize required components
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;

        // Retrieve input actions from the PlayerInput component
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];

        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        CheckGroundStatus();
        HandleMovement();
        ApplyGravity();
    }

    private void CheckGroundStatus()
    {
        // Check if the player is grounded and reset vertical velocity if needed
        isGrounded = controller.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
    }

    private void HandleMovement()
    {
        MovePlayer(MovementDirection());
        RotateTowardsCamera();

        // Makes the player jump
        if (CanJump())
        {
            Jump();
        }
    }


    private void ApplyGravity()
    {
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private bool CanJump()
    {
        return jumpAction.triggered && isGrounded;
    }

    public Vector3 MovementDirection()
    {
        // Read movement input
        Vector2 input = moveAction.ReadValue<Vector2>();
        animator.SetFloat("InputX", input.x);
        animator.SetFloat("InputY", input.y);
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0f;
        return move;
    }

    private void MovePlayer(Vector3 moveDirection)
    {
        // Move the player based on input and speed
        controller.Move(moveDirection * Time.deltaTime * playerSpeed);

        // Rotate the player to face the direction of movement
        if (moveDirection != Vector3.zero)
        {
            transform.forward = moveDirection;
        }
    }

    private void Jump()
    {
        playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
    }

    private void RotateTowardsCamera()
    {
        // Rotate towards camera postion.
        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
