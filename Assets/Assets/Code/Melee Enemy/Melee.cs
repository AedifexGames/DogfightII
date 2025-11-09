using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Melee : Agent
{

    // Settings
    public Transform target;
    public float curSpeed = 5f;
    public float speedBoost = 2f; // PROTOTYPE
    private Rigidbody2D _rb;
    private Vector2 startPosition;

    // Tracker
    private int numBoosts = 0; // PROTOTYPE
    private float timeElapsed = 0f;

    public override void Initialize()
    {
        _rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    public override void OnEpisodeBegin()
    {
        // Reset agent
        transform.position = startPosition;
        _rb.linearVelocity = Vector2.zero;
        numBoosts = 0;
        timeElapsed = 0f;

        curSpeed = 5f;

        // Spawn asteroids?
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Positions
        sensor.AddObservation(transform.position);
        sensor.AddObservation(target.position);

        // Speed
        sensor.AddObservation(_rb.linearVelocity);

        // Boosts
        sensor.AddObservation(numBoosts / 5f);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Continuous X, Y movement
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];

        Vector2 move = new Vector2(moveX, moveY).normalized * curSpeed;
        _rb.AddForce(move, ForceMode2D.Force);

        timeElapsed += Time.deltaTime;

        // Reward
        float distToTarget = Vector2.Distance(transform.position, target.position);
        float reward = -distToTarget * 0.001f;
        AddReward(reward);
        AddReward(-0.0005f); // time penalty?
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Asteroid"))
        {
            numBoosts++;
            curSpeed += speedBoost;
            AddReward(1.0f);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Player"))
        {
            // Reward (speed & time)
            float finalSpeed = _rb.linearVelocity.magnitude;
            float finalReward = 5f + finalSpeed * 0.5f - timeElapsed * 0.1f + numBoosts;
            AddReward(finalReward);

            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Horizontal");
        continuousActions[1] = Input.GetAxis("Vertical");
    }
}
