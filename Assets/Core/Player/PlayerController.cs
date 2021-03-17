using UnityEngine;

namespace AtaCetin
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float swerveSpeed = 0.5f;
        [SerializeField] private float maxSwerveAmount = 1f;

        [SerializeField] private float rotatingStickForceAmount = 10f;
        [SerializeField] private float moveSpeed = 10f;

        [SerializeField] private GameObject spawnPoint;
        private GameManager _gameManager;
        private PlayerInputSystem _playerInputSystem;
        private Animator _anim;

        private bool _bMovementEnabled = true;
        private Rigidbody _rigidBody;

        private GameObject _targetPoint;

        private void Awake()
        {
            _playerInputSystem = GetComponent<PlayerInputSystem>();
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        private void Start()
        {
            _anim = GetComponentInChildren<Animator>();
            _rigidBody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (!_bMovementEnabled) return;
            _rigidBody.WakeUp();
            if (transform.position.y < -10) Respawn();

            float yurumeHizi = 0;

            //ekrana dokunuyorsa yürü
            if (_playerInputSystem.GetScreenTouching)
                yurumeHizi = moveSpeed;

            var swerveAmount = Time.deltaTime * swerveSpeed * _playerInputSystem.MoveFactorX;

            swerveAmount = Mathf.Clamp(swerveAmount, -maxSwerveAmount, maxSwerveAmount);

            transform.Translate(swerveAmount, 0, yurumeHizi * Time.deltaTime);
        }


        private void LateUpdate()
        {
            if (_playerInputSystem.GetScreenTouching && _bMovementEnabled)
                _anim.SetBool("moving", true);
            else
                _anim.SetBool("moving", false);
        }

        private void OnCollisionEnter(Collision cllsn)
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
                    _rigidBody.AddForce(cllsn.contacts[0].normal.x * rotatingStickForceAmount, 0,
                        cllsn.contacts[0].normal.z * rotatingStickForceAmount, ForceMode.Impulse);
                    break;
                case "switchState":
                    if(_gameManager.GetActiveState() < 1)
                        _gameManager.SetState(1);
                    else
                        Finish();
                    
                    break;
            }
        }

        private void Finish()
        {
           //  TODO bitince ekran göster
        }
        
        
        public void SetMovement(bool var)
        {
            _bMovementEnabled = var;
        }

        public void SetPlayerLocation(Vector3 location)
        {
            transform.position = location;
        }


        public void SetSpawnPoint(GameObject spawnPoint)
        {
            this.spawnPoint = spawnPoint;
        }

        private void Respawn()
        {
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.angularVelocity = Vector3.zero;
            if (spawnPoint == null) spawnPoint = _gameManager.GetSpawnPoint();
            transform.position = spawnPoint.transform.position;
        }

        public void LerpCamera(float lerpTime, bool toTargetEye)
        {
            transform.Find("CameraBoy").localPosition = Vector3.Lerp(transform.Find("CameraBoy").localPosition,
                toTargetEye ? transform.Find("TargetEye").localPosition : new Vector3(0, 10.12f, -10.71f), lerpTime);
        }
    }
}