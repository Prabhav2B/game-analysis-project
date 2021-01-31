using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Speedometer : MonoBehaviour
{
    public Rigidbody target;

    public float maxSpeed = 0.0f; // The maximum speed of the target ** IN KM/H **

    public float minSpeedArrowAngle;
    public float maxSpeedArrowAngle;

    [Header("UI")]
    public TMP_Text speedLabel; // The label that displays the speed;
    public RectTransform arrow; // The arrow in the speedometer

    private float speed = 0.0f;

    [SerializeField] private bool isBland = false;


    private void Update()
    {
        speed = target.velocity.magnitude / maxSpeed * 240.0f;
        if (isBland)
        {
            if (speed > 1f)
            {
                speedLabel.text = "Racing!";
            }
            else
            {
                speedLabel.text = "Stationary";
            }
        }
        else
        {
            // 3.6f to convert in kilometers
            // ** The speed must be clamped by the car controller **
            
            if (speedLabel != null)
                speedLabel.text = ((int)speed) + " km/h";
            if (arrow != null)
                arrow.localEulerAngles =
                    new Vector3(0, 0, Mathf.Lerp(minSpeedArrowAngle, maxSpeedArrowAngle, speed / 260.0f));
        }
    }
}
