using System.Linq;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class EnemyAgent : Agent
{
    private GameObject[] CheckPoints;
    private Rigidbody rigidBody;

    private void Start()
    {
        //checkpointleri taginden al
        CheckPoints = GameObject.FindGameObjectsWithTag("checkpoint");
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //eğer aşağı düşerse -ödül ver(makine ögrenmesi icin) ve bastan başlat
        if (transform.position.y < -10f)
        {
            SetReward(-10f);
            EndEpisode();
        }
    }

    //çarpma algılama
    private void OnCollisionEnter(Collision cllsn)
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
                //kazandı !!
                SetReward(+10f);
                //EndEpisode();
                this.enabled = false;
                break;
        }
    }

    //tetiklere girmesi durumunda
    private void OnTriggerEnter(Collider cllsn)
    {
        //checkpointlerse puan ekle ki machinelearning iyi birsey yaptıgını algılasın
        switch (cllsn.gameObject.tag)
        {
            case "checkpoint":
                //checkpointi kapa
                cllsn.gameObject.SetActive(false);
                SetReward(+5f);
                break;
        }
    }

    
    public override void OnEpisodeBegin()
    {
        //baslangic noktasına ısınlan
        transform.position = new Vector3(0, 0, -2.679f);

        //checkpoint listsindeki noktaları tekrar aktifleştir
        foreach (var item in CheckPoints) item.SetActive(true);
    }
    
    public override void CollectObservations(VectorSensor sensor)
    {
        //hedefleri burdan ögreniyor.
        
        //aktif olan ilk checkpoint i al
        var gelencheck = CheckPoints.OrderBy(x => x.name).FirstOrDefault(x => x.activeSelf);

        //checkpointin lokasyonunu al. null gelirse parantezden çık
        if (gelencheck is null) return;
        var point = gelencheck.transform.position;
        //hedefi ekle
        sensor.AddObservation(point);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var moveX = actions.ContinuousActions[0];
        var moveZ = actions.ContinuousActions[1];

        var moveSpeed = 10f;

        if (moveZ > 0)
            transform.position += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
        else
            transform.position += new Vector3(moveX, 0, 0f) * Time.deltaTime * moveSpeed;
        
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //elle ögretmek icin gereken bir adım. klavyeyle veri girmeyi saglıyor.
        var conAct = actionsOut.ContinuousActions;

        if (Input.GetKey(KeyCode.W)) conAct[1] = 1f;
        if (Input.GetKey(KeyCode.A)) conAct[0] = -1f;
        if (Input.GetKey(KeyCode.D)) conAct[0] = 1f;
        if (Input.GetKey(KeyCode.S)) conAct[1] = 0f;
        if (Input.GetKey(KeyCode.Q)) conAct[0] = 0f;
    }
}