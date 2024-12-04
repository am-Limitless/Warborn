using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchVirtualCam : MonoBehaviour
{
    #region Fields and References

    [Header("Player Input and Actions")]
    [SerializeField] private PlayerInput playerInput;
    private InputAction aimAction;

    [Header("Cinemachine Camera Settings")]
    //private CinemachineVirtualCamera virtualCamera;
    private int priorityBoostAmount = 10;

    [Header("UI Canvases")]
    [SerializeField] Canvas thirdPersonCanvas;
    [SerializeField] Canvas aimCanvas;

    #endregion

    #region Unity Lifecycle Methods

    private void Awake()
    {
        //virtualCamera = GetComponent<CinemachineVirtualCamera>();
        aimAction = playerInput.actions["Aim"];
        aimCanvas.enabled = false;
    }

    private void OnEnable()
    {
        aimAction.performed += _ => StartAim();
        aimAction.canceled += _ => CancelAim();
    }

    private void OnDisable()
    {
        aimAction.performed -= _ => StartAim();
        aimAction.canceled -= _ => CancelAim();
    }

    #endregion

    #region Aim Handling Methods

    private void StartAim()
    {
        //virtualCamera.Priority += priorityBoostAmount;
        aimCanvas.enabled = true;
        thirdPersonCanvas.enabled = false;
    }

    private void CancelAim()
    {
        //virtualCamera.Priority -= priorityBoostAmount;
        aimCanvas.enabled = false;
        thirdPersonCanvas.enabled = true;
    }

    #endregion
}




