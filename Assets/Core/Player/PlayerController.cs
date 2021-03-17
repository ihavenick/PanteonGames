using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtaCetin
{
    public class PlayerController : MonoBehaviour
    {

        Animator anim;
        private PlayerInputSystem _playerInputSystem;
        [SerializeField] private float swerveSpeed = 0.5f;
        [SerializeField] private float maxSwerveAmount = 1f;

        [SerializeField] private float rotatingStickForceAmount = 10f;
        [SerializeField] private float moveSpeed = 10f;

        private GameObject TargetPoint;
        private GameManager _gameManager;
        private Rigidbody rigidBody;

        private bool bMovementEnabled = true;

        [SerializeField] private GameObject spawnPoint;

        private void Start()
        {
            anim = GetComponentInChildren<Animator>();
            rigidBody = GetComponent<Rigidbody>();
        }

        public void SetMovement(bool var)
        {
            bMovementEnabled = var;
        }

        private void Awake()
        {
            _playerInputSystem = GetComponent<PlayerInputSystem>();
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        public void SetPlayerLocation(Vector3 location) => this.transform.position = location;


        public void SetSpawnPoint(GameObject SpawnPoint)
        {
            spawnPoint = SpawnPoint;
        }

        private void Respawn()
        {
            //todo stage check
            rigidBody.velocity = Vector3.zero;
            rigidBody.angularVelocity = Vector3.zero;
            if (spawnPoint == null) spawnPoint = _gameManager.GetSpawnPoint();
            transform.position = spawnPoint.transform.position;

        }

        private void Update()
        {
            if (bMovementEnabled)
            {
                rigidBody.WakeUp();
                if (this.transform.position.y < -10)
                    Respawn();

                float yurumeHizi = 0;

                //ekrana dokunuyorsa yürü
                if (_playerInputSystem.GetScreenTouching)
                    yurumeHizi = moveSpeed;

                float swerveAmount = Time.deltaTime * swerveSpeed * _playerInputSystem.MoveFactorX;

                swerveAmount = Mathf.Clamp(swerveAmount, -maxSwerveAmount, maxSwerveAmount);

                transform.Translate(swerveAmount, 0, yurumeHizi * Time.deltaTime);
            }
            else
            {

            }


        }


        private void LateUpdate()
        {
            if (_playerInputSystem.GetScreenTouching && bMovementEnabled)
                anim.SetBool("moving", true);
            else
                anim.SetBool("moving", false);
        }

        public void LerpCamera(float lerpTime)
        {
            transform.Find("CameraBoy").position = Vector3.Lerp(this.transform.Find("CameraBoy").position,
                this.transform.Find("TargetEye").position, lerpTime);
        }

        void OnCollisionEnter(Collision cllsn)
        {
            //neyle çarpıştı
            switch (cllsn.gameObject.tag)
            {
                case "obstacle":
                    //sabit engelse baştan başlat
                    Respawn();
                    break;
                case "rotatingStick":
                    //eğer dönen cubuksa güç uygula
                    rigidBody.AddForce(cllsn.contacts[0].normal.x * rotatingStickForceAmount, 0,
                        cllsn.contacts[0].normal.z * rotatingStickForceAmount, ForceMode.Impulse);
                    break;
                case "switchState":
                    _gameManager.SetState(1);
                    break;
                default:
                    break;
            }

        }
    }
}