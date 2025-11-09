using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AccelerationForce : MonoBehaviour 
{
    // Goals:
    // To change:
    // Angle
    // Velocity
    // Vertical pull

    private List<CircleCollider2D> _colliders = new List<CircleCollider2D>();
    private List<float> _radii = new List<float>();
    private List<float> _masses = new List<float>();

    [SerializeField] private AnimationCurve _gravVelPull;
    [SerializeField] private float _speedupStrength;
    [SerializeField] private AnimationCurve _gravAnglePull;
    [SerializeField] private float _curveStrength;
    [SerializeField] private AnimationCurve _gravVertPull;
    [SerializeField] private float _pullStrength;

    // Target information variables
    private List<Vector3> _targetPos = new List<Vector3>();
    private List<Vector2> _targetLinVel = new List<Vector2>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Collect target information
        _targetPos.Add(new Vector3(
            collision.transform.position.x,
            collision.transform.position.y,
            collision.transform.position.z
        ));
        _targetLinVel.Add(new Vector2(
            collision.attachedRigidbody.linearVelocityX,
            collision.attachedRigidbody.linearVelocityY
        ));
    }


    private void Start()
    {
        _colliders.Add(GetComponent<CircleCollider2D>());
        int lastIndex = _colliders.Count - 1;
        _radii.Add(_colliders[lastIndex].radius);
        _masses.Add(_colliders[lastIndex].attachedRigidbody.mass);
    }


    private float calcDistNormTime()
    {
        for (int i=0; i< _colliders.Count; i++)
        {
            // Calculate distance variables
            Vector3 distVec = _targetPos[i] - transform.position;
            float dist = distVec.magnitude;
            if (dist == 0) { return 1; }
            float distNorm = dist / _radii[i];
            float distTime = 1 - distNorm;

            // 0 at radius, 1 at center
            // Will NOT work with return in for loop HERE
            return distTime;
        }
        return 0;
    }


    private Quaternion calcAngleDelta()
    {
        for (int i = 0; i < _colliders.Count; i++)
        {
            float time = calcDistNormTime();
            float strength = _gravAnglePull.Evaluate(time);

            Vector3 targetToCenterVec = transform.position - _targetPos[i];
            Quaternion curRotation = Quaternion.LookRotation(_targetLinVel[i]);
            Quaternion targetRotation = Quaternion.LookRotation(targetToCenterVec);

            // Rotation needed
            Quaternion rotationNeeded = targetRotation * Quaternion.Inverse(curRotation);
            rotationNeeded.Normalize();

            float maxAngle = Vector3.Angle(_targetLinVel[i], targetToCenterVec);

            // Convert rotation angle to degrees
            rotationNeeded.ToAngleAxis(out float angle, out Vector3 axis);

            // Clamp rotation
            float scaledAngle = Mathf.Min(angle * strength, maxAngle);

            // Reconstruct scaled rotation
            Quaternion scaledRotation = Quaternion.AngleAxis(scaledAngle, axis);

            // Scaled rotation in world space
            // Will NOT work with return in for loop HERE
            return scaledRotation * curRotation;
        }        
        return Quaternion.identity;
    }


    private Vector2 calcVelocityDelta()
    {
        for (int i = 0; i < _colliders.Count; i++)
        {
            float time = calcDistNormTime();

            float strength = _gravVelPull.Evaluate(time);

            float strengthReduced = (_masses[i] != 0) ? strength / _masses[i] : strength;

            Vector2 directionNorm = _targetLinVel[i].normalized;

            // Will NOT work with return in for loop HERE
            return directionNorm * strengthReduced * _speedupStrength;
        }
        return Vector2.zero;
    }


    private float calcVertPullDelta()
    {
        for (int i = 0; i < _colliders.Count; i++)
        {
            float time = calcDistNormTime();

            float strength = _gravVertPull.Evaluate(time);

            float strengthReduced = (_masses[i] != 0) ? strength / _masses[i] : strength;

            // Will NOT work with return in for loop HERE
            return strengthReduced * _pullStrength;
        }
        return 0;
    }
}
