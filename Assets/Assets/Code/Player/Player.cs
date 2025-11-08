using UnityEngine;
using System;
using UnityEngine.XR;
public class Player : MonoBehaviour
{
    #region Variables
    private InputManager _inputManager;
    private Rigidbody2D _rb;
    [SerializeField] private float _turnSpeed;
    [SerializeField] private float _baseMoveSpeed;
    private float _currentMoveSpeed;
    [SerializeField] private float _maxAngleOffset = 60f;
    private UnityEventOnTimer _projectileTimer;

    //Charge Variables
    [SerializeField] private float _targetChargeAmount;
    [SerializeField] private float _chargeSpeed = 1;
    private float _currentChargeAmount;
    private bool _charging = false;
    private bool _charged = false;
    [SerializeField] private AnimationCurve _chargeSlowDown;
    [SerializeField] private float _chargeMinSpeed;
    private float _cachedMoveSpeed;

    //Dash Variables
    [SerializeField] private float _fullyChargedDashSpeed;
    [SerializeField] private float _minChargeToDash;
    [SerializeField] private float _partialChargeDashDifference;
    [SerializeField] private float _baseDashSpeed;
    [SerializeField] private AnimationCurve _dashLifetimeSpeed;
    [SerializeField] private float _totalDashTime;
    private float _currentDashTime;
    private bool _dashing;

    //PLACEHOLDER
    private SpriteRenderer _spriteRenderer;
    #endregion

    private void Start()
    {
        _inputManager = InputManager.instance;
        _inputManager.onSpacebarPressed += SpacebarStart;
        _inputManager.onSpacebarRelease += SpacebarEnd;
        _rb = GetComponent<Rigidbody2D>();
        _projectileTimer = GetComponent<UnityEventOnTimer>();

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = Color.orange;

        _currentMoveSpeed = _baseMoveSpeed;
    }

    private void Update()
    {
        _rb.angularVelocity = _turnSpeed * _inputManager.TurnInput();
        _rb.linearVelocity = _currentMoveSpeed * transform.up;
        ClampAngle();
        if (_charging && !_charged)
        {
            _currentChargeAmount += Time.deltaTime * _chargeSpeed;
            if (_currentChargeAmount > _targetChargeAmount)
            {
                _charged = true;
                _spriteRenderer.color = Color.green;
            }
            float dif = _cachedMoveSpeed - _chargeMinSpeed;
            _currentMoveSpeed = _chargeMinSpeed + _chargeSlowDown.Evaluate(_currentChargeAmount / _targetChargeAmount) * dif;
        }

        if (_dashing)
        {
            _currentDashTime += Time.deltaTime;
            if (_currentDashTime > _totalDashTime)
            {
                EndDash();
            }
            //Update the speed with the curve
            float dif = _cachedMoveSpeed - _baseMoveSpeed;
            _currentMoveSpeed = _baseMoveSpeed + _dashLifetimeSpeed.Evaluate(_currentDashTime / _totalDashTime) * dif;
            Debug.Log("Dashing at the speed of: " + _currentMoveSpeed);
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
        EndDash();
        _projectileTimer.Pause();
        _currentChargeAmount = 0;
        _charging = true;
        _charged = false;
        _spriteRenderer.color = Color.yellow;
        _cachedMoveSpeed = _currentMoveSpeed;
    }

    private void SpacebarEnd()
    {
        _currentMoveSpeed = _baseMoveSpeed;
        _spriteRenderer.color = Color.orange;
        if (_currentChargeAmount >= _minChargeToDash)
        {
            StartDash();
        }
        _projectileTimer.Unpause();
        _charging = false;
        _charged = false;
    }
    
    private float GetDashSpeed()
    {
        //Partially charged (over threshold)
        if (_currentChargeAmount > _minChargeToDash && !_charged)
        {
            float progressTime = _currentChargeAmount - _minChargeToDash;
            return _baseDashSpeed + (progressTime * (_partialChargeDashDifference / (_targetChargeAmount - _minChargeToDash)));
        }
        //Fully charged
        else if (_charged)
        {
            return _fullyChargedDashSpeed;
        }
        return _baseMoveSpeed;
    }

    private void StartDash()
    {
        _currentMoveSpeed = GetDashSpeed();
        _dashing = true;
        _currentDashTime = 0;
        _cachedMoveSpeed = _currentMoveSpeed;
        _spriteRenderer.color = Color.blue;
    }

    private void EndDash()
    {
        Debug.Log("Ending Dash");
        _dashing = false;
        _currentMoveSpeed = _baseMoveSpeed;
        _spriteRenderer.color = Color.orange;
    }
}
