using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class GeometryBugFinder : Agent
{
    Rigidbody rb;

    //public Transform agentTarget; // donde esta el bug

    private HashSet<Vector3> visitedPositions = new HashSet<Vector3>();

    public grid_score_detection gridScore;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
        sensor.AddObservation(transform.position);
        //sensor.AddObservation(agentTarget.position);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        float axisBiasPenalty = Mathf.Abs(moveX - moveZ) * -0.01f;
        AddReward(axisBiasPenalty);

        Vector3 movement = new Vector3(moveX, 0, moveZ) * 5f;
        rb.AddForce(movement, ForceMode.VelocityChange);

        //float distanceToTarget = Vector3.Distance(transform.position, agentTarget.position);
        //AddReward(-distanceToTarget * 0.01f);

        if (visitedPositions.Add(transform.position))
        {
            AddReward(0.001f);
        }
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Horizontal");
        continuousActions[1] = Input.GetAxis("Vertical");
    }
}