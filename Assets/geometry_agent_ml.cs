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

    public Transform startPos;


    private float punishmentMultiplier = 1f;

    public float rayLength = 5f;
    public bool showDebugRays = true;

    public float[] distances = new float[8];

    //public override void Initialize()
    //{
    //    base.Initialize();
    //    // Randomize starting behavior
    //    GetComponent<BehaviorParameters>().BehaviorType =
    //        Random.Range(0, 2) == 0 ? BehaviorType.Default : BehaviorType.HeuristicOnly;
    //}

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GetComponent<Rigidbody>().collisionDetectionMode =
            CollisionDetectionMode.ContinuousDynamic;
        //Physics.simulationMode = true;
        Physics.gravity = new Vector3(0, -100f, 0);
    }

    void Update()
    {
        Vector3 origin = transform.position;

        for (int i = 0; i < 8; i++)
        {
            float angle = i * 45f;
            Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;

            RaycastHit hit;
            if (Physics.Raycast(origin, direction, out hit, rayLength))
            {
                distances[i] = hit.distance / rayLength; // normalizamos
            }

            if (showDebugRays)
            {
                Color rayColor = hit.collider != null ? Color.red : Color.green;
                Debug.DrawRay(origin, direction * rayLength, rayColor);
            }
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
        //sensor.AddObservation(transform.position);
        foreach (float distance in distances)
        {
            sensor.AddObservation(distance);
        }
        //sensor.AddObservation(agentTarget.position);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        //float axisBiasPenalty = Mathf.Abs(moveX - moveZ) * -0.01f;
        //AddReward(axisBiasPenalty);

        Vector3 movement = new Vector3(moveX, 0, moveZ) * 5f;
        rb.AddForce(movement, ForceMode.VelocityChange);

        //float distanceToTarget = Vector3.Distance(transform.position, agentTarget.position);
        //AddReward(-distanceToTarget * 0.01f);

        //if (visitedPositions.Add(transform.position))
        //{
        //    AddReward(0.001f);
        //}

        try
        {
            if (gridScore.CurrentCell(transform.position) == true)
            {
                punishmentMultiplier += 0.001f;
            }

            else
            {
                punishmentMultiplier = 1f;
            }

            if (gridScore.CheckWalked(transform.position) == false)
            {
                AddReward(1f);
            }

            else
            {
                AddReward(-0.001f * punishmentMultiplier);
            }

            gridScore.MarkWalked(transform.position);
        }

        catch
        {
            // pass
        }

        //if (StepCount > 1000 && GetCumulativeReward() < -500)
        //{
        //    EndEpisode();
        //}
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Horizontal");
        continuousActions[1] = Input.GetAxis("Vertical");
    }

    public override void OnEpisodeBegin()
    {
        transform.position = startPos.position;

        gridScore.ResetGrid();
    }
}