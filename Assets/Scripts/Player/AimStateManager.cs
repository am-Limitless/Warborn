
using UnityEngine;

public class AimStateManager : MonoBehaviour
{
    #region Fields and References

    [Header("Axis States for Aiming")]
    public Cinemachine.AxisState xAxis;
    public Cinemachine.AxisState yAxis;

    [Header("Camera Follow Position")]
    [SerializeField] Transform camFollowPos;

    #endregion

    #region Unity Lifecycle Methods

    private void Update()
    {
        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);
    }

    private void LateUpdate()
    {
        camFollowPos.localEulerAngles = new Vector3(yAxis.Value, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis.Value, transform.eulerAngles.z);
    }

    #endregion
}
