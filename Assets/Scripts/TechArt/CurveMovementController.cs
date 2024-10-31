using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class CurveMovementController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] jointObjects;
    [Header("Parameters")]
    [SerializeField] private float rotationRange;
    [SerializeField] private float strenghtRotation;
    
    private Vector3 _lastPos;
    private Quaternion _targetRotation;
    private Vector3 _velocity;

    private void Start() => _lastPos = transform.position;
    
    private void Update()
    {
        var rotationDirection = Vector3.Cross(Vector3.up, _velocity.normalized);
        rotationDirection.x = 0;
        
        _targetRotation.eulerAngles = rotationDirection * rotationRange;
        

        foreach (var o in jointObjects)
        {
            o.transform.localRotation  = Quaternion.Slerp(o.transform.localRotation,_targetRotation, Time.deltaTime * strenghtRotation);
        }
        
    }

    private void FixedUpdate()
    {
        _velocity = transform.position - _lastPos;
        _lastPos = transform.position;
    }
}
