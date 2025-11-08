using UnityEngine;
using System;
using UnityEngine.XR;
public class Player : MonoBehaviour
{
    #region Variables
    private InputManager _inputManager;
    private Rigidbody2D _rb;

    //Movement Settings
    [SerializeField] private float _turnSpeed;
    [SerializeField] private float _baseMoveSpeed;
    private float _currentMoveSpeed;
    [SerializeField] private float _maxAngleOffset = 60f;
    private UnityEventOnTimer _projectileTimer;
    [SerializeField] private float _realignmentTime;
    private float _currRealignmentTime;

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

    //Offset Control Variables
    private CameraTarget _cameraTarget;
    [SerializeField] private float _minOffset;
    [SerializeField] private float _maxOffset;
    [SerializeField] private float _targetSpeedForMinOffset;
    [SerializeField] private float _targetSpeedForMaxOffset;
    [SerializeField] private AnimationCurve _offsetLerp;

    //Gravitation Variables
    [SerializeField] private float _centerGravStrength = 5;
    [SerializeField] private AnimationCurve _gravitationToCenter;

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

        _cameraTarget = CameraTarget.instance;
    }

    private void Update()
    {
        // Set movement settings
        _rb.angularVelocity = _turnSpeed * _inputManager.TurnInput();
        _rb.linearVelocity = _currentMoveSpeed * transform.up;
        ClampAngle();
        ClampYAxis();
        RealignPath();

        // Apply gravitation forces
        _rb.linearVelocityY += GravitationToCenter();

        // Handle dashing and charging
        Charge();
        Dash();
        CalculateOffset(); // move forward with speed


    }

    private void ClampAngle()
    {
        float z = transform.rotation.eulerAngles.z;
        if (z > 180f) z -= 360f;
        transform.rotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x, 
            transform.rotation.eulerAngles.y, 
            Mathf.Clamp(z, -90f - _maxAngleOffset, -90f + _maxAngleOffset)
        );
    }

    private void ClampYAxis()
    {
        float edgesDist = 10;
        transform.position = new Vector3(
            transform.position.x,
            Mathf.Clamp(transform.position.y, -edgesDist, edgesDist),
            transform.position.z
        );
    }

    private void RealignPath()
    {
        if (_inputManager.TurnInput() == 0 && _inputManager.SpacebarPressed())
        {
            // Get direction and change
            float rotateDegree = 0.5f;
            float angleOffset = transform.rotation.eulerAngles.z - 270;

            if (angleOffset == 0) return;

            float pointedDirection = angleOffset / Math.Abs(angleOffset);

            // Check edge case
            rotateDegree = (Math.Abs(angleOffset) < rotateDegree) ? Math.Abs(angleOffset) : rotateDegree;
            
            // Calculate change
            float z = transform.rotation.eulerAngles.z + (rotateDegree * (-pointedDirection));

            // Apply change
            transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles.x,
                transform.rotation.eulerAngles.y,
                z
            );
        }
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

    private void Charge()
    {
        // If charging (not charged)
        if (_charging && !_charged)
        {
            // Increase charge
            _currentChargeAmount += Time.deltaTime * _chargeSpeed;

            // Finish charging
            if (_currentChargeAmount > _targetChargeAmount)
            {
                _charged = true; // prevent further increase
                _spriteRenderer.color = Color.green; // PROTOTYPE marker
            }

            // Slow down movement
            float dif = _cachedMoveSpeed - _chargeMinSpeed;
            _currentMoveSpeed = _chargeMinSpeed + _chargeSlowDown.Evaluate(_currentChargeAmount / _targetChargeAmount) * dif;
        }
    }

    private void Dash()
    {
        // Current dashing
        if (_dashing)
        {
            // Set movement settinggs
            _rb.angularVelocity = (_turnSpeed / 2) * _inputManager.TurnInput(); // half turning speed

            // Count down dash duration
            _currentDashTime += Time.deltaTime;
            if (_currentDashTime > _totalDashTime)
            {
                EndDash();
            }

            // Speed up movement and dropoff
            float dif = _cachedMoveSpeed - _baseMoveSpeed;
            _currentMoveSpeed = _baseMoveSpeed + _dashLifetimeSpeed.Evaluate(_currentDashTime / _totalDashTime) * dif;
            //Debug.Log("Dashing at the speed of: " + _currentMoveSpeed);
        }
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
        //Debug.Log("Ending Dash");
        _dashing = false;
        _currentMoveSpeed = _baseMoveSpeed;
        _spriteRenderer.color = Color.orange;
    }

    private void CalculateOffset()
    {
        // Velocities
        float v_0 = _baseMoveSpeed; // base speed
        float v_x = _currentMoveSpeed; // current speed
        float v_m = _targetSpeedForMinOffset; // goal speed

        // Positions
        float B = _minOffset; // goal position
        float A = _maxOffset;

        // Differences
        float dif_pos = B - A; // distance to goal position
        float dif_vel = v_m - v_0; // difference between current and goal speeds

        // Times
        float time = v_x - v_0; // amount faster than base speed
        float time_norm = time / dif_vel; // time normalized [0,1]

        // Set offset
        _cameraTarget.SetOffset(A + (_offsetLerp.Evaluate(time_norm) * dif_pos));
    }

    private float GravitationToCenter()
    {
        float y = transform.position.y;
        float edgeDist = 10;
        float yOffset = Math.Abs(y);
        float yOffsetNorm = yOffset / edgeDist;

        float flipFactor = y / Math.Abs(y);

        return _centerGravStrength * _gravitationToCenter.Evaluate(yOffsetNorm) * -flipFactor;
    }
}
