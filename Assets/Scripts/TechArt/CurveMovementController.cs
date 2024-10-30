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
        
        _targetRotation.eulerAngles = rotationDirection * rotationRange;
        

        for (int i = 0; i < jointObjects.Length; ++i)
        {
            jointObjects[i].transform.rotation = Quaternion.Slerp(jointObjects[i].transform.rotation,_targetRotation, Time.deltaTime * strenghtRotation);
        }
        
    }

    private void FixedUpdate()
    {
        _velocity = transform.position - _lastPos;
        _lastPos = transform.position;
    }
}
