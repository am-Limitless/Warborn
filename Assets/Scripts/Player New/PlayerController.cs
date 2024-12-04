using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerController : MonoBehaviourPunCallbacks, IDamageable
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
    public Transform player;
    public Transform gun;
    private Transform cameraTransform;
    public float rotationSpeed = 3f;

    [Header("Items Reference")]
    [SerializeField] Item[] items;
    private int itemIndex;
    private int previousItemIndex = -1;

    private float verticalLookRotation;
    private bool isGrounded;
    private Vector3 currentMovementVelocity;
    private Vector3 movementAmount;

    private Rigidbody rb;
    private new PhotonView photonView;

    [Header("Health Setttings")]
    const float maxHealth = 100f;
    float currentHealth = maxHealth;
    [SerializeField] Image healthbarImage;
    [SerializeField] GameObject ui;

    PlayerManager playerManager;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        photonView = GetComponent<PhotonView>();

        playerManager = PhotonView.Find((int)photonView.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    private void Start()
    {
        HandlePhotonView();

        // Assign cameraTransform only if this is the local player
        if (photonView.IsMine && cameraHolder != null)
        {
            // Find the Cinemachine camera under this player's cameraHolder
            //CinemachineVirtualCamera virtualCamera = cameraHolder.GetComponentInChildren<CinemachineVirtualCamera>();
            //if (virtualCamera != null)
            //{
            //    virtualCamera.Priority = 10; // Ensure this camera has the highest priority
            //    //virtualCamera.enabled = true;
            //}

            // Assign the cameraTransform to this player's camera
            //cameraTransform = virtualCamera?.transform;
        }
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        HandleCameraRotation();
        RotateGunToCamera();
        HandleMovementInput();
        HandleJumpInput();
        ItemSwitch();
        UseItem();
        CheckFall();
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        PerformMovement();
    }

    #endregion

    #region Camera Methods

    void HandleCameraRotation()
    {
        if (player == null || cameraTransform == null) return;


        Vector3 cameraForward = cameraTransform.forward;

        cameraForward.y = 0;
        cameraForward.Normalize();

        if (cameraForward.magnitude < 0.01f) return;

        Quaternion targetRotation = Quaternion.LookRotation(cameraForward);

        player.rotation = Quaternion.Slerp(player.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void RotateGunToCamera()
    {
        if (gun == null || cameraTransform == null) return;

        // Get the camera's pitch rotation (up/down)
        Vector3 cameraForward = cameraTransform.forward;

        // Calculate the gun's target rotation based on the camera's forward direction
        Quaternion targetRotation = Quaternion.LookRotation(cameraForward);

        // Apply the target rotation to the gun
        gun.rotation = Quaternion.Slerp(gun.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    #endregion

    #region Movement Methods

    void HandleMovementInput()
    {
        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

        float targetSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
        movementAmount = Vector3.SmoothDamp(movementAmount, inputDirection * targetSpeed, ref currentMovementVelocity, movementSmoothingTime);
    }

    void HandleJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void PerformMovement()
    {
        rb.MovePosition(rb.position + transform.TransformDirection(movementAmount) * Time.fixedDeltaTime);
    }

    #endregion

    #region Photon Methods

    private void HandlePhotonView()
    {
        if (photonView.IsMine)
        {
            //CinemachineVirtualCamera virtualCamera = cameraHolder.GetComponentInChildren<CinemachineVirtualCamera>();
            //if (virtualCamera != null)
            //{
            //    virtualCamera.Priority = 10; // Active for local player
            //    //virtualCamera.enabled = true;
            //}
        }
        else
        {
            //CinemachineVirtualCamera virtualCamera = cameraHolder.GetComponentInChildren<CinemachineVirtualCamera>();
            //if (virtualCamera != null)
            //{
            //    virtualCamera.Priority = 0; // Deactivate for remote players
            //    //virtualCamera.enabled = false;
            //}
            Destroy(rb); // Prevent remote players from using Rigidbody
            Destroy(ui);
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

    #region Items Methods

    private void EquipItem(int _index)
    {
        if (_index == previousItemIndex)
        {
            return;
        }

        itemIndex = _index;

        items[itemIndex].itemGameObject.SetActive(true);

        if (previousItemIndex != -1)
        {
            items[previousItemIndex].itemGameObject.SetActive(false);
        }

        previousItemIndex = itemIndex;

        if (photonView.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex", itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!photonView.IsMine && targetPlayer == photonView.Owner)
        {
            EquipItem((int)changedProps["itemIndex"]);
        }
    }

    private void ItemSwitch()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                EquipItem(i);
                break;
            }
        }
    }

    private void UseItem()
    {
        if (Input.GetMouseButtonDown(0))
        {
            items[itemIndex].Use();
        }
    }

    #endregion

    #region Health Methods

    public void TakeDamage(float damage)
    {
        photonView.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        currentHealth -= damage;

        healthbarImage.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void CheckFall()
    {
        if (transform.position.y < -10f)
        {
            Die();
        }
    }
    void Die()
    {
        playerManager.Die();
    }

    #endregion
}