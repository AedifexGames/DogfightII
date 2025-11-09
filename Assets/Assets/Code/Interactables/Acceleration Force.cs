using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Physics;
using UnityEngine.PlayerLoop;
using System;
using static Physics.Rigidbody2DManager;
public class AccelerationForce : MonoBehaviour 
{
    // Goals:
    // To change:
    // Angle
    // Velocity
    // Vertical pull

    private CircleCollider2D _collider;
    private float _radius;
    private float _mass;

    [SerializeField] private AnimationCurve _gravVelPull;
    [SerializeField] private float _speedupStrength;
    [SerializeField] private AnimationCurve _gravAnglePull;
    [SerializeField] private float _curveStrength;
    [SerializeField] private AnimationCurve _gravVertPull;
    [SerializeField] private float _pullStrength;

    [SerializeField] private float _decayRate = 2f;

    private List<Rigidbody2DManager> _targets = new List<Rigidbody2DManager>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Rigidbody2DManager>() != null )
        {
            Rigidbody2DManager manager = collision.GetComponent<Rigidbody2DManager>();
            _targets.Add(manager);

            if (!manager.HasForce("Orbit"))
            {
                manager.AddForce("Orbit", Vector2.zero, 0f, 0f);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Rigidbody2DManager>() != null)
        {
            Rigidbody2DManager rb = collision.GetComponent<Rigidbody2DManager>();
            _targets.Remove(rb);

            // Enable decay when leaving 
            Force orbitalForce = rb.GetForce("Orbit");
            if (orbitalForce != null)
            {
                orbitalForce.DecayRate = _decayRate;
            }
            else
            {
                // Removal
                rb.RemoveForce("Orbit");
            }
        }
    }
    private void Start()
    {
        _collider = GetComponent<CircleCollider2D>();
        _radius = _collider.radius;
        _mass = (_collider.attachedRigidbody != null && _collider.attachedRigidbody.mass != 0f) ? _collider.attachedRigidbody.mass : 1f;
    }

    private float calcDistNormTime(int i)
    {
        // Calculate distance variables
        Vector3 distVec = _targets[i].transform.position - transform.position;
        float dist = distVec.magnitude;
        if (dist == 0) { return 1; }

        float distNorm = dist / _radius;
        float distTime = Mathf.Clamp01(1 - distNorm);

        // 0 at radius, 1 at center
        return distTime;
    }


    private float calcAngleDelta(int i)
    {
        Vector2 targetToCenter = (Vector2)(transform.position - _targets[i].transform.position);

        Vector2 velocity = _targets[i].GetRigidbody2D().linearVelocity;

        if (velocity == Vector2.zero)
        {
            velocity = Vector2.right;
        }

        float angleDiff = Vector2.SignedAngle(velocity, targetToCenter);

        return angleDiff * _gravAnglePull.Evaluate(calcDistNormTime(i)) * _curveStrength;

        //Vector3 targetToCenterVec = transform.position - _targets[i].transform.position;
        //Quaternion curRotation = Quaternion.LookRotation(_targets[i].GetRigidbody2D().linearVelocity);
        //Quaternion targetRotation = Quaternion.LookRotation(targetToCenterVec);

        //// Rotation needed
        //Quaternion rotationNeeded = targetRotation * Quaternion.Inverse(curRotation);
        //rotationNeeded.Normalize();

        //float maxAngle = Vector3.Angle(_targets[i].GetRigidbody2D().linearVelocity, targetToCenterVec);

        //// Convert rotation angle to degrees
        //rotationNeeded.ToAngleAxis(out float angle, out Vector3 axis);

        //// Clamp rotation
        //float scaledAngle = Mathf.Min(angle * strength, maxAngle);

        //// Reconstruct scaled rotation
        //Quaternion scaledRotation = Quaternion.AngleAxis(scaledAngle, axis);

        //Debug.Log("Real angle " + scaledRotation * curRotation);

        //// Scaled rotation in world space
        //return scaledRotation * curRotation;
    }


    private Vector2 calcVelocityDelta(int i)
    {
        Vector2 targetToCenter = (Vector2)(transform.position - _targets[i].transform.position);
        
        float time = calcDistNormTime(i);
        
        float strength = _gravVelPull.Evaluate(time);

        Vector2 velocityIncrease = targetToCenter.normalized * strength * _speedupStrength / _mass;

        Vector2 velIncAbs = new Vector2(
            Mathf.Abs(velocityIncrease.x),
            velocityIncrease.y
        );

        return velIncAbs;
    }


    private float calcVertPullDelta(int i)
    {
        float time = calcDistNormTime(i);

        float strength = _gravVertPull.Evaluate(time);

        float strengthReduced = strength / _mass;

        float flip = (_targets[i].transform.position.y <= transform.position.y) ? -1f : 1f;

        return strengthReduced * _pullStrength * flip;
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < _targets.Count; i++)
        {
            float angularVelocity = calcAngleDelta(i);
            Vector2 linearVelocity = calcVelocityDelta(i);
            float vertPull = calcVertPullDelta(i);
            linearVelocity += Vector2.up * vertPull;

            _targets[i].AdjustForce(
                "Orbit",
                linearVelocity * Time.fixedDeltaTime,
                angularVelocity * Time.fixedDeltaTime);
        }
    }

    public void Update()
    {
        for (int i = 0; i < _targets.Count; i++)
        {
            float angularVelocity = calcAngleDelta(i);
            Vector2 linearVelocity = calcVelocityDelta(i);
            float vertPull = calcVertPullDelta(i);
            linearVelocity += Vector2.up * vertPull;
            _targets[i].AdjustForce(
                "Orbit", 
                linearVelocity * Time.fixedDeltaTime, 
                angularVelocity * Time.fixedDeltaTime);
        }
    }
}
