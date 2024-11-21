using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    // ----- Input Fields -----
    private ThirdPersonActionsAsset playerActionAsset;
    private InputAction move;

    // ----- Movement Fields -----
    [Header("Movement Settings")]
    private Rigidbody rb;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;
    private Vector3 playerVelocity;
    private bool isGrounded;

    // ----- Character Controller Reference -----
    private CharacterController controller;

    // ----- Camera Reference -----
    [SerializeField] private Camera playerCamera;

    private void Awake()
    {
        controller = this.GetComponent<CharacterController>();
        playerActionAsset = new ThirdPersonActionsAsset();
    }

    private void OnEnable()
    {
        playerActionAsset.Player.Jump.started += DoJump;
        playerActionAsset.Player.Enable();
        move = playerActionAsset.Player.Move;
    }

    private void OnDisable()
    {
        playerActionAsset.Player.Jump.started -= DoJump;
        playerActionAsset.Player.Disable();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        ApplyGravity();
        //LookAt();
    }

    private void HandleMovement()
    {
        Vector2 inputVector = move.ReadValue<Vector2>();

        // Calculate movement direction relative to the camera's orientation
        Vector3 moveDirection = inputVector.x * GetCameraRight(playerCamera) + inputVector.y * GetCameraForward(playerCamera);
        moveDirection.Normalize();

        // Move the character using CharacterController.Move()
        controller.Move(moveDirection * movementSpeed * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // Apply gravity over time
        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    //private void LookAt()
    //{
    //    Vector2 inputVector = move.ReadValue<Vector2>();
    //    Vector3 direction = new Vector3(inputVector.x, 0, inputVector.y);

    //    if (direction.sqrMagnitude > 0.1f)
    //    {
    //        // Rotate the character to face movement direction
    //        Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
    //        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720 * Time.deltaTime); // Adjust rotation speed as needed
    //    }
    //}

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    // ----- Camera Direction -----
    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    // ----- Jump Handling -----
    private void DoJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}