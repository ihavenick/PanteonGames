using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class EnemyAgent : Agent
{
    [SerializeField] private Transform TargetTransform;
    private Rigidbody rigidBody;

    private GameObject[] CheckPoints;

    private void Start()
    {
        CheckPoints = GameObject.FindGameObjectsWithTag("checkpoint");
        rigidBody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        transform.position = new Vector3(0, 0, -2.679f);

        foreach (var item in CheckPoints)
        {
            item.SetActive(true);
        }
        
        //base.OnEpisodeBegin();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        var gelencheck = CheckPoints.OrderBy(x => x.name).FirstOrDefault(x=>x.activeSelf);

        var point = new Vector3(0, 0, 0);
        
        point = gelencheck != null ? gelencheck.transform.position : GameObject.FindWithTag("switchState").transform.position;

        // Debug.Log("New target : " + gelencheck.name + " position :" + gelencheck.transform.position.ToString());
        sensor.AddObservation(point);
        //sensor.AddObservation(transform.position);
        //sensor.AddObservation(TargetTransform.position);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        float moveSpeed = 10f;

        if (moveZ > 0)
            transform.position += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
        else
        {
            transform.position += new Vector3(moveX, 0, 0f) * Time.deltaTime * moveSpeed;
        }
        
        //transform.position += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> conAct = actionsOut.ContinuousActions;
        //conAct[0] = Input.GetAxisRaw("Horizontal");
        //conAct[1] = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.W))
        {
            conAct[1] = 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            conAct[0] = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            conAct[0] = 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            conAct[1] = 0f;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            conAct[0] = 0f;
        }
        
    }

    private void Update()
    {
        if (transform.position.y < -10f)
        {
            SetReward(-10f);
            EndEpisode();
        }
    }
    
    void OnCollisionEnter(Collision cllsn)
    {
        switch (cllsn.gameObject.tag)
        {
            case "obstacle":
                //sabit engelse baştan başlat
                SetReward(-1f);
                EndEpisode();
                break;
            case "rotatingStick":
                //eğer dönen cubuksa güç uygula
                rigidBody.AddForce(cllsn.contacts[0].normal.x * 10f, 0,
                    cllsn.contacts[0].normal.z * 10f, ForceMode.Impulse);
                break;
            case "switchState":
                Debug.Log("Kazandii!!!!!");
                SetReward(+10f);
                EndEpisode();
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider cllsn)
    {
        switch (cllsn.gameObject.tag)
        {
            case "checkpoint":
                cllsn.gameObject.SetActive(false);
                //Debug.Log("Deactivating " + cllsn.gameObject.name + " " + cllsn.gameObject.activeInHierarchy);
                SetReward(+5f);
                break; 
        }
    }
}
