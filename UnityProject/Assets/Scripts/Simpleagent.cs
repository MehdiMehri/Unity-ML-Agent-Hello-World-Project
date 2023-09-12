using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class Simpleagent : Agent
{
    [SerializeField] Transform startPos;
    [SerializeField] Transform player;
    [SerializeField] Transform target;
    [SerializeField] Transform trap;
    [SerializeField] float speed = 3;

    // visualize
    [SerializeField] MeshRenderer floor;
    [SerializeField] Material winMat;
    [SerializeField] Material loseMat;

    public override void OnActionReceived(ActionBuffers actions)
    {
        float x = actions.ContinuousActions[0];
        float z = actions.ContinuousActions[1];

        player.position += new Vector3(x, 0, z) * Time.deltaTime * speed;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(player.localPosition);
        sensor.AddObservation(target.localPosition);
        sensor.AddObservation(trap.localPosition);
    }

    public override void OnEpisodeBegin()
    {
        player.localPosition = new Vector3(Random.RandomRange(-3f,3),0,Random.Range(1.5f,3.2f));
        target.localPosition = new Vector3(Random.RandomRange(-3f,3),0, 0.16f);
        trap.localPosition = new Vector3(Random.RandomRange(-3f,3),0, -2.6f);

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actions = actionsOut.ContinuousActions;

        actions[0] = Input.GetAxis("Horizontal");
        actions[1] = Input.GetAxis("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("walls"))
        {
            AddReward(-1);
            floor.material = loseMat;
            OnEpisodeBegin();
        }
        else if (other.CompareTag("target"))
        {
            AddReward(1);
            floor.material = winMat;
            OnEpisodeBegin();
        }
        if (other.CompareTag("trap"))
        {
            AddReward(-1);
            floor.material = loseMat;
            OnEpisodeBegin();
        }

    }
}
