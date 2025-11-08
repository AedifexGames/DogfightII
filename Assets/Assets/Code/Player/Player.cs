using UnityEngine;
using System;
using UnityEngine.XR;
public class Player : MonoBehaviour
{
    private InputManager _inputManager;
    private Rigidbody2D _rb;
    [SerializeField] private float _turnSpeed;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _maxAngleOffset = 60f;
    private void Start()
    {
        _inputManager = InputManager.instance;
        _inputManager.onSpacebarPressed += SpacebarStart;
        _inputManager.onSpacebarRelease += SpacebarEnd;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _rb.angularVelocity = _turnSpeed * _inputManager.TurnInput();
        _rb.linearVelocity = _moveSpeed * transform.up;
        ClampAngle();
    }

    private void ClampAngle()
    {
        float z = transform.rotation.eulerAngles.z;
        if (z > 180f) z -= 360f;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Clamp(z, -90f - _maxAngleOffset, -90f + _maxAngleOffset));
    }

    private void SpacebarStart()
    {
        
    }

    private void SpacebarEnd()
    {

    }
}
