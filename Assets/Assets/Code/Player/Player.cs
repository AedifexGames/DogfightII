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
    private UnityEventOnTimer _projectileTimer;

    [SerializeField] private float _targetChargeAmount;
    [SerializeField] private float _chargeSpeed = 1;
    private float _currentChargeAmount;
    private bool _charging = false;
    private bool _charged = false;

    private void Start()
    {
        _inputManager = InputManager.instance;
        _inputManager.onSpacebarPressed += SpacebarStart;
        _inputManager.onSpacebarRelease += SpacebarEnd;
        _rb = GetComponent<Rigidbody2D>();
        _projectileTimer = GetComponent<UnityEventOnTimer>();
    }

    private void Update()
    {
        _rb.angularVelocity = _turnSpeed * _inputManager.TurnInput();
        _rb.linearVelocity = _moveSpeed * transform.up;
        ClampAngle();
        if (_charging && !_charged)
        {
            _currentChargeAmount += Time.deltaTime * _chargeSpeed;
            if (_currentChargeAmount > _targetChargeAmount)
                _charged = true;
        }
    }

    private void ClampAngle()
    {
        float z = transform.rotation.eulerAngles.z;
        if (z > 180f) z -= 360f;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Clamp(z, -90f - _maxAngleOffset, -90f + _maxAngleOffset));
    }

    private void SpacebarStart()
    {
        _projectileTimer.Pause();
        _currentChargeAmount = 0;
        _charging = true;
        _charged = false;
    }

    private void SpacebarEnd()
    {
        if (_charged)
        {
            Dash();
        }
        _projectileTimer.Unpause();
        _charging = false;
        _charged = false;
    }

    private void Dash()
    {

    }
}
