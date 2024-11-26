using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables

    [Header("Movement Settings")]
    [SerializeField] float mouseSensitivity;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float movementSmoothingTime;


    [Header("Camera Settings")]
    [SerializeField] GameObject cameraHolder;

    private float verticalLookRotation;
    private bool isGrounded;
    private Vector3 currentMovementVelocity;
    private Vector3 movementAmount;

    private Rigidbody rb;
    private PhotonView photonView;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        HandlePhotonView();
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        HandleCameraRotation();
        HandleMovementInput();
        HandleJumpInput();
    }

    private void FixedUpdate()
    {
        PerformMovement();
    }

    #endregion

    #region Camera Methods

    void HandleCameraRotation()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        // Horizontal rotation
        transform.Rotate(Vector3.up * mouseX);

        // Vertical rotation with clamping
        verticalLookRotation += mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    #endregion

    #region Movement Methods

    /// <summary>
    /// Handles player movement based on input.
    /// </summary>
    void HandleMovementInput()
    {
        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

        float targetSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
        movementAmount = Vector3.SmoothDamp(movementAmount, inputDirection * targetSpeed, ref currentMovementVelocity, movementSmoothingTime);
    }

    /// <summary>
    /// Handles the jump action when the player is grounded.
    /// </summary>
    void HandleJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// Moves the player rigidbody in the fixed update.
    /// </summary>
    private void PerformMovement()
    {
        rb.MovePosition(rb.position + transform.TransformDirection(movementAmount) * Time.fixedDeltaTime);
    }

    #endregion

    #region Photon Methods

    /// <summary>
    /// Manages the setup for non-local players.
    /// Destroys unnecessary components for better performance.
    /// </summary>
    private void HandlePhotonView()
    {
        if (!photonView.IsMine)
        {
            Destroy(cameraHolder.GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
        }
    }

    #endregion

    #region Grounded State

    /// <summary>
    /// Updates the grounded state for jumping logic.
    /// </summary>
    public void SetGroundedState(bool _grounded)
    {
        isGrounded = _grounded;
    }

    #endregion

}
