using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AtaCetin
{
    public class GameManager : MonoBehaviour
    {
        private PlayerController _playerController;
        private GameObject _spawnPoint;
        private StateManager _stateManager;
        [SerializeField] private GameObject _canvas;
        [SerializeField] public GameObject enemyPrefab;

        private bool _lerpToSpawn;
        private float _lerpValue;
        private bool toTargetEye = true;
        private GameObject player;
        public Text currentRankingList;
        private bool showRankings = false;

        private List<GameObject> participantList;

        private void Start()
        {
            participantList = new List<GameObject>();
            player = GameObject.FindWithTag("Player");
            participantList.Add(player);
            _playerController = player.GetComponent<PlayerController>();
            _canvas = GameObject.Find("Filled Pie");

            Debug.Log("GameManager Init");
            _stateManager = new StateManager();
            _stateManager.AddState("Running");
            _stateManager.AddState("Painting");
            _stateManager.AddState("Racing");

            SetState(0);
        }

        private void Update()
        {
            if (_lerpToSpawn)
            {
                _lerpValue += Time.deltaTime / 0.5f;
                var playerPosition = _playerController.transform.position;
                var newPosition = _spawnPoint.transform.position;
                if (playerPosition == newPosition)
                {
                    _lerpToSpawn = false;
                    return;
                }
                _playerController.SetPlayerLocation(Vector3.Lerp(playerPosition, newPosition, _lerpValue));
                _playerController.LerpCamera(_lerpValue,toTargetEye);
            }
            else
            {
                _lerpValue = 0;
            }

            if (showRankings)
            {
                participantList.Sort((x,y) => x.transform.position.z.CompareTo(y.transform.position.z));
                currentRankingList.text = 10 - participantList.IndexOf(player) + ".ci sıra";
            }
        }

        public GameObject GetSpawnPoint()
        {
            return _spawnPoint;
        }

        public void SetState(int ID)
        {
            switch (ID)
            {
                case 0:
                    _canvas.SetActive(false);
                    _playerController.SetMovement(true);
                    _spawnPoint = GameObject.Find("Spawn01");
                    currentRankingList.text = "2. bölüme geçmek için parkuru bitir";
                    _playerController.SetSpawnPoint(_spawnPoint);
                    break;
                case 1:
                    _playerController.SetMovement(false);
                    _spawnPoint = GameObject.Find("Spawn02");
                    _playerController.SetSpawnPoint(_spawnPoint);
                    _stateManager.SetActiveState(1);
                    _lerpToSpawn = true;
                    currentRankingList.text = "Bölümü geçmek için %70 boyayın";
                    _canvas.SetActive(true);
                    break;
                case 2:
                    _stateManager.SetActiveState(2);
                    _spawnPoint = GameObject.Find("Spawn01");
                    _playerController.SetSpawnPoint(_spawnPoint);
                    _canvas.SetActive(false);
                    _playerController.SetMovement(true);
                    toTargetEye = false;
                    _lerpToSpawn = true;
                    showRankings = true;
                    for (int i = 0; i < 9; i++)
                    {
                        var a = Instantiate(enemyPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                        participantList.Add(a);
                    }
                    break;
                
                default:
                    _stateManager.SetActiveState(0);
                    break;
            }
        }

        public int GetActiveState()
        {
            return _stateManager.GetActiveState?.ID ?? 0;
        }
    }

    public class StateManager
    {
        public StateManager()
        {
            GetStates = new List<State>();
        }

        private List<State> GetStates { get; }

        public State GetActiveState => GetStates.FirstOrDefault(x => x.isActive);
        
        public void AddState(string name)
        {
            var state = new State
            {
                StateName = name,
                ID = GetStates.Count,
                isActive = false,
            };
            GetStates.Add(state);
        }
        public void SetActiveState(int id)
        {
            foreach (var state in GetStates.Where(x => x.isActive)) state.isActive = false;

            FindByID(id).isActive = true;
        }
        private State FindByID(int id)
        {
            return GetStates.FirstOrDefault(x => x.ID == id);
        }
    }


    public class State
    {
        public int ID { get; set; }
        public string StateName { get; set; }
        public bool isActive { get; set; }
    }
}