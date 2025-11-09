using UnityEngine;
using System;
using UnityEngine.XR;
using Physics;
public class Player : MonoBehaviour
{
    #region Variables
    private InputManager _inputManager;
    private Rigidbody2DManager _rb;

    //Movement Settings
    [SerializeField] private float _turnSpeed;
    [SerializeField] private float _baseMoveSpeed;
    private float _currentMoveSpeed;
    [SerializeField] private float _maxAngleOffset = 60f;
    private UnityEventOnTimer _projectileTimer;
    [SerializeField] private float rotateDegree = 270f;
    [SerializeField] private float mobilityFactor;

    //Charge Variables
    [SerializeField] private float _targetChargeAmount;
    [SerializeField] private float _chargeSpeed = 1;
    private float _currentChargeAmount;
    private bool _charging = false;
    private bool _charged = false;
    [SerializeField] private AnimationCurve _chargeSlowDown;
    [SerializeField] private float _chargeMinSpeed;
    private float _cachedMoveSpeed;
    [SerializeField] private Transform _chargeIndicator;
    [SerializeField] private Vector2 _indicatorInitialScale;
    [SerializeField] private Vector2 _indicatorFinalScale;

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
    private bool _calculateOffset = true;
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
        _rb = GetComponent<Rigidbody2DManager>();
        _rb.AddForce("Player Input", Vector2.zero, 0, 0);
        _projectileTimer = GetComponent<UnityEventOnTimer>();

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = Color.orange;

        _currentMoveSpeed = _baseMoveSpeed;

        _cameraTarget = CameraTarget.instance;
    }

    private void Update()
    {
        // Set movement settings
        float angularVelocity = (_inputManager.SpacebarPressed()) 
            ? _turnSpeed * _inputManager.TurnInput()
            : _turnSpeed * 2 * _inputManager.TurnInput();
        if (_dashing) angularVelocity = (_turnSpeed / 2) * _inputManager.TurnInput(); // half turning speed

        ClampAngle();
        ClampYAxis();
        RealignPath();

        // Mobility factor and gravitation forces
        /*_rb.linearVelocity = new Vector2(
            _rb.linearVelocityX,
            (_rb.linearVelocityY + GravitationToCenter()) * mobilityFactor
        );*/

        // Apply gravitation forces
        //_rb.linearVelocityY += GravitationToCenter();

        // Handle dashing and charging
        Charge();
        Dash();
        CalculateOffset(); // move forward with speed
        Vector2 linearVelocity = _currentMoveSpeed * transform.up;
        _rb.SetForce("Player Input", linearVelocity, angularVelocity);
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
        float edgesDist = 9.5f;
        transform.position = new Vector3(
            transform.position.x,
            Mathf.Clamp(transform.position.y, -edgesDist, edgesDist),
            transform.position.z
        );
    }

    private void RealignPath()
    {
        if (_dashing) return;
        if (_inputManager.TurnInput() == 0 && !_inputManager.SpacebarPressed())
        {
            // Get direction and change
            float angleOffset = transform.rotation.eulerAngles.z - 270;

            if (Mathf.Abs(angleOffset) < 2f) { transform.rotation = Quaternion.Euler(0,0,270); return; }

            float pointedDirection = angleOffset / Math.Abs(angleOffset);

            // Check edge case
            float amt = (Math.Abs(angleOffset) < Time.deltaTime * rotateDegree) ? Math.Abs(angleOffset) : rotateDegree;
            
            // Calculate change
            float z = transform.rotation.eulerAngles.z + Time.deltaTime * (amt * (-pointedDirection));

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
        _chargeIndicator.localScale = _indicatorInitialScale;
    }

    private void SpacebarEnd()
    {
        _chargeIndicator.gameObject.SetActive(false);
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

            if (_currentChargeAmount > _minChargeToDash)
            {
                _chargeIndicator.gameObject.SetActive(true);
            }
            // Finish charging
            if (_currentChargeAmount > _targetChargeAmount)
            {
                _charged = true; // prevent further increase
                _spriteRenderer.color = Color.green; // PROTOTYPE marker
            }

            // Slow down movement
            float dif = _cachedMoveSpeed - _chargeMinSpeed;
            _currentMoveSpeed = _chargeMinSpeed + _chargeSlowDown.Evaluate(_currentChargeAmount / _targetChargeAmount) * dif;
            _chargeIndicator.transform.localScale = Vector2.Lerp(_indicatorInitialScale, _indicatorFinalScale, _currentChargeAmount / _targetChargeAmount);
        }
    }

    private void Dash()
    {
        // Current dashing
        if (_dashing)
        {
            // Count down dash duration
            _currentDashTime += Time.deltaTime;
            if (_currentDashTime > _totalDashTime)
            {
                EndDash();
            }
            if (_currentDashTime > _totalDashTime / 20) { EnableOffset(); }

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
        DisableOffset();
    }

    private void EndDash()
    {
        //Debug.Log("Ending Dash");
        _dashing = false;
        _currentMoveSpeed = _baseMoveSpeed;
        _spriteRenderer.color = Color.orange;
        EnableOffset();
    }

    private void CalculateOffset()
    {
        if (!_calculateOffset) return;
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

        if (yOffsetNorm == 0) return 0;

        float flipFactor = y / Math.Abs(y);

        return _centerGravStrength * _gravitationToCenter.Evaluate(yOffsetNorm) * -flipFactor;
    }

    private void DisableOffset()
    {
        _calculateOffset = false;
        _cameraTarget.enabled = false;
    }

    private void EnableOffset()
    {
        _calculateOffset = true;
        _cameraTarget.enabled = true;
    }

    public bool Dashing { get { return _dashing; } }
}
