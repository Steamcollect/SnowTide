using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestVehicleMovement : MonoBehaviour
{
    public Vector2 minMaxMoveSpeed;
    public float turnMoveSpeed;

    public float driftAngle;
    public TrailRenderer[] tyreMarks;

    public Vector2 minMaxRotation;

    [Header("References")]
    public Rigidbody rb;
    public Slider turnSlider;

    Vector3 velocity;

    private void Update()
    {
        float currentRot = Mathf.Lerp(minMaxRotation.x, minMaxRotation.y, turnSlider.value);
        if (Mathf.Abs(currentRot) > driftAngle) Emitting(true);
        else Emitting(false);

        void Emitting(bool isEmitting)
        {
            foreach (var t in tyreMarks)
            {
                t.emitting = isEmitting;
            }
        }

        transform.rotation = Quaternion.Euler(0, currentRot, 0);

        velocity.z = Mathf.Lerp(minMaxMoveSpeed.x, minMaxMoveSpeed.y, Mathf.Abs((turnSlider.value * 2) - 1));
        velocity.x = ((turnSlider.value * 2) -1) * turnMoveSpeed;

        rb.velocity = velocity;
    }
}