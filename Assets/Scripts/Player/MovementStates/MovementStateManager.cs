using UnityEngine;
using UnityEngine.InputSystem;


public class MovementStateManager : MonoBehaviour
{
    #region Fields and References

    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] float roataionSpeed = 5f;

    [Header("State Machine")]
    public IdleState Idle = new IdleState();
    public WalkState Walk = new WalkState();
    public CrounchState Crounch = new CrounchState();
    public RunState Run = new RunState();
    private MovementBaseState currentState;

    [Header("Input and Animation")]
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    [HideInInspector] public Vector3 dir;
    [HideInInspector] public Animator animator;

    [Header("References")]
    CharacterController controller;
    private Transform cameraTransform;

    // Physics and Ground Check
    private bool isGrounded;
    private Vector3 playerVelocity;

    #endregion

    #region Unity Lifecycle Methods

    private void Start()
    {
        // Initialize required components
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;
        animator = GetComponentInChildren<Animator>();

        // Retrieve input actions from the PlayerInput component
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];

        SwicthState(Idle);
    }

    private void Update()
    {
        GetDirectionAndMove();
        ApplyGravity();

        currentState.UpdateState(this);
    }

    #endregion

    #region State Management

    public void SwicthState(MovementBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    #endregion

    #region Movement Logic

    public void GetDirectionAndMove()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();

        dir = new Vector3(input.x, 0, input.y);

        animator.SetFloat("InputX", input.x);
        animator.SetFloat("InputY", input.y);


        dir = dir.x * cameraTransform.right.normalized + dir.z * cameraTransform.forward.normalized;
        dir.y = 0;

        controller.Move(dir.normalized * moveSpeed * Time.deltaTime);

        // Rotate towards camera direction
        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, roataionSpeed * Time.deltaTime);
    }

    #endregion

    #region Gravity and Ground Check

    private bool CheckGroundStatus()
    {
        // Check if the player is grounded and reset vertical velocity if needed
        isGrounded = controller.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ApplyGravity()
    {
        if (!CheckGroundStatus())
        {
            playerVelocity.y += gravityValue * Time.deltaTime;
        }
        else if (playerVelocity.y < 0)
        {
            playerVelocity.y = -2;
        }
        controller.Move(playerVelocity * Time.deltaTime);
    }

    #endregion
}
