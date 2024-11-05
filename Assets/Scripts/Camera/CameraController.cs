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

    [Space(10)]
    [SerializeField] Camera cam;
    [SerializeField] RSO_Camera rso_Cam;
    [SerializeField] RSE_FloatEvent rse_VehicleRotation;

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

    private void OnEnable()
    {
        rse_VehicleRotation.action += SetTargetRotation;
    }
    private void OnDisable()
    {
        rse_VehicleRotation.action -= SetTargetRotation;
    }
}