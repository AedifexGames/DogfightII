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
    [SerializeField] private AnimationCurve gravPull;

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

    private float calcStrength()
    {
        // Calculate distance variables
        Vector3 distVec = targetPos - transform.position;
        float dist = distVec.magnitude;
        if (dist == 0) { return 1; }
        float distNorm = dist / radius;
        float distTime = 1 - distNorm;

        float strength = gravPull.Evaluate( distTime );


        return (mass != 0) ? strength / mass : strength;
    }

    private void calcAngleDelta()
    {

    }

    private void calcVelocityDelta()
    {

    }

    private void calcVertPullDelta()
    {

    }
}
