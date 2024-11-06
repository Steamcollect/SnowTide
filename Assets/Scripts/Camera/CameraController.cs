using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target;

    [SerializeField] Vector3 posOffset;
    [SerializeField] float timeOffset;
    Vector3 velocity = Vector3.zero;

    [SerializeField] float rotationTime;
    [SerializeField] float maxRotation;

    Quaternion targetRotation = Quaternion.identity;

    [SerializeField] GameObject speedLine;

    [Space(10)]
    [SerializeField] Camera cam;
    [SerializeField] RSO_Camera rso_Cam;
    [SerializeField] RSE_FloatEvent rse_VehicleRotation;
    [SerializeField] RSE_ToggleSpeedLines rseToggleSpeedLines;

    private void Start()
    {
        rso_Cam.Value = cam;
    }

    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target.position + posOffset, ref velocity, timeOffset);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationTime * Time.deltaTime);
    }

    void SetTargetRotation(float rotation)
    {
        float y = Mathf.Lerp(0, maxRotation, Mathf.Abs(rotation));
        y *= rotation < 0 ? -1 : 1;

        targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, y, transform.rotation.eulerAngles.z);
    }

    void ToggleSpeedLines(bool isActive)
    {
        //speedLine.SetActive(isActive);
    }

    private void OnEnable()
    {
        rse_VehicleRotation.action += SetTargetRotation;
        rseToggleSpeedLines.action += ToggleSpeedLines;
    }
    private void OnDisable()
    {
        rse_VehicleRotation.action -= SetTargetRotation;
        rseToggleSpeedLines.action -= ToggleSpeedLines;
    }
}