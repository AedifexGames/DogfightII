using Unity.VisualScripting;
using UnityEngine;

public class AccelerationForce : MonoBehaviour 
{
    // Goals:
    // To change:
    // Angle
    // Velocity
    // Vertical pull

    private CircleCollider2D _collider;
    private float radius;
    private float mass;
    [SerializeField] private AnimationCurve gravVelPull;
    [SerializeField] private float speedupStrength;
    [SerializeField] private AnimationCurve gravAnglePull;
    [SerializeField] private float curveStrength;
    [SerializeField] private AnimationCurve gravVertPull;
    [SerializeField] private float pullStrength;

    // Target information variables
    Vector3 targetPos;
    Quaternion targetRot;
    Vector2 targetLinVel;
    float targetAngVel;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Collect target information
        Vector3 targetPos = new Vector3(
            collision.transform.position.x,
            collision.transform.position.y,
            collision.transform.position.z
        );
        Quaternion targetRot = new Quaternion(
            collision.transform.rotation.x,
            collision.transform.rotation.y,
            collision.transform.rotation.z,
            collision.transform.rotation.w
        );
        Vector2 targetLinVel = new Vector2(
            collision.attachedRigidbody.linearVelocityX,
            collision.attachedRigidbody.linearVelocityY
        );
        float targetAngVel = collision.attachedRigidbody.angularVelocity;

    }


    private void Start()
    {
        _collider = GetComponent<CircleCollider2D>();
        radius = _collider.radius;
        mass = _collider.attachedRigidbody.mass;
    }


    private float calcDistNormTime()
    {
        // Calculate distance variables
        Vector3 distVec = targetPos - transform.position;
        float dist = distVec.magnitude;
        if (dist == 0) { return 1; }
        float distNorm = dist / radius;
        float distTime = 1 - distNorm;

        // 0 at radius, 1 at center
        return distTime;
    }


    private Quaternion calcAngleDelta()
    {
        float time = calcDistNormTime();
        float strength = gravAnglePull.Evaluate(time);

        Vector3 targetToCenterVec = transform.position - targetPos;
        Quaternion curRotation = Quaternion.LookRotation(targetLinVel);
        Quaternion targetRotation = Quaternion.LookRotation(targetToCenterVec );

        // Rotation needed
        Quaternion rotationNeeded = targetRotation * Quaternion.Inverse( curRotation );
        rotationNeeded.Normalize();

        float maxAngle = Vector3.Angle(targetLinVel, targetToCenterVec);

        // Convert rotation angle to degrees
        rotationNeeded.ToAngleAxis(out float angle, out Vector3 axis);

        // Clamp rotation
        float scaledAngle = Mathf.Min(angle * strength, maxAngle);

        // Reconstruct scaled rotation
        Quaternion scaledRotation = Quaternion.AngleAxis(scaledAngle, axis);

        // Scaled rotation in world space
        return scaledRotation * curRotation;
        
    }


    private float calcVelocityDelta()
    {
        float time = calcDistNormTime();

        float strength = gravVelPull.Evaluate(time);

        float strengthReduced = (mass != 0) ? strength / mass : strength;

        return strengthReduced * speedupStrength;
    }


    private float calcVertPullDelta()
    {
        float time = calcDistNormTime();

        float strength = gravVertPull.Evaluate(time);

        float strengthReduced = (mass != 0) ? strength / mass : strength;

        return strengthReduced * pullStrength;
    }
}
