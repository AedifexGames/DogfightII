using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Physics;
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

    private List<Rigidbody2DManager> _targets = new List<Rigidbody2DManager>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Rigidbody2DManager>() != null )
            _targets.Add(collision.GetComponent<Rigidbody2DManager>());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Rigidbody2DManager>() != null)
            _targets.Remove(collision.GetComponent<Rigidbody2DManager>());
    }
    private void Start()
    {
        _collider = GetComponent<CircleCollider2D>();
        _radius = _collider.radius;
        _mass = _collider.attachedRigidbody.mass;
    }


    private float calcDistNormTime(int i)
    {
            // Calculate distance variables
            Vector3 distVec = _targets[i].transform.position - transform.position;
            float dist = distVec.magnitude;
            if (dist == 0) { return 1; }
            float distNorm = dist / _radius;
            float distTime = 1 - distNorm;

            // 0 at radius, 1 at center
            // Will NOT work with return in for loop HERE
            return distTime;
    }


    private Quaternion calcAngleDelta(int i)
    {
            float time = calcDistNormTime(i);
            float strength = _gravAnglePull.Evaluate(time);

            Vector3 targetToCenterVec = transform.position - _targets[i].transform.position;
            Quaternion curRotation = Quaternion.LookRotation(_targets[i].GetRigidbody2D().linearVelocity);
            Quaternion targetRotation = Quaternion.LookRotation(targetToCenterVec);

            // Rotation needed
            Quaternion rotationNeeded = targetRotation * Quaternion.Inverse(curRotation);
            rotationNeeded.Normalize();

            float maxAngle = Vector3.Angle(_targets[i].GetRigidbody2D().linearVelocity, targetToCenterVec);

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


    private Vector2 calcVelocityDelta(int i)
    {
            float time = calcDistNormTime(i);

            float strength = _gravVelPull.Evaluate(time);

            float strengthReduced = (_mass != 0) ? strength / _mass : strength;

            Vector2 directionNorm = _targets[i].GetRigidbody2D().linearVelocity.normalized;

            // Will NOT work with return in for loop HERE
            return directionNorm * strengthReduced * _speedupStrength;
    }


    private float calcVertPullDelta(int i)
    {
            float time = calcDistNormTime(i);

            float strength = _gravVertPull.Evaluate(time);

            float strengthReduced = (_mass != 0) ? strength / _mass : strength;

            // Will NOT work with return in for loop HERE
            return strengthReduced * _pullStrength;
    }

    public void Update()
    {
        for (int i = 0; i < _targets.Count; i++)
        {
            float angularVelocity = calcAngleDelta(i).eulerAngles.z;
            Vector2 linearVelocity = calcVelocityDelta(i);
            linearVelocity += Vector2.up * calcVertPullDelta(i);
            _targets[i].GetComponent<Rigidbody2DManager>()?.SetForce("Orbit", linearVelocity, angularVelocity);
        }
    }
}
