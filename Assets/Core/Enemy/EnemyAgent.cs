using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class EnemyAgent : Agent
{
    [SerializeField] private Transform TargetTransform;

    public override void OnEpisodeBegin()
    {
        transform.position = new Vector3(0, 0, -2.679f);
        
        base.OnEpisodeBegin();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(TargetTransform.position);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
       // float moveZ = actions.ContinuousActions[1];

        float moveSpeed = 1f;
        transform.position += new Vector3(moveX, 0, 10f) * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
       // ActionSegment<float> conAct = actionsOut.ContinuousActions;
      //  conAct[0] = Input.GetAxisRaw("Horizontal");
      //  conAct[1] = Input.GetAxisRaw("Vertical");
    }

    private void Update()
    {
        if (transform.position.y < -10f)
        {
            SetReward(-1f);
            EndEpisode();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "obstacle":
                //sabit engelse baştan başlat
                SetReward(-1f);
                EndEpisode();
                break;
            case "rotatingStick":
                //eğer dönen cubuksa güç uygula
               // rigidBody.AddForce(cllsn.contacts[0].normal.x * rotatingStickForceAmount, 0,
                   // cllsn.contacts[0].normal.z * rotatingStickForceAmount, ForceMode.Impulse);
                break;
            case "Kazan":
                SetReward(+1f);
                EndEpisode();
                break;
            case "checkpoint":
                SetReward(+1f);
                break;
            default:
                break;
        }

       
        
        
    }
}
