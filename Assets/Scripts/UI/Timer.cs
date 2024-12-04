using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] TMP_Text timerText;

    public float maxTime;

    private void FixedUpdate()
    {
        maxTime += Time.deltaTime;

        timerText.text = maxTime.ToString();
    }
}
