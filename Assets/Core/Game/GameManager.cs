using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameManager : MonoBehaviour
{
    private StateManager _stateManager;
    private PlayerController _playerController;
    private float lerpValue;
    private GameObject _spawnPoint;
    private bool lerpToSpawn =false;

    [SerializeField] public List<GameObject> Stage1Obstacles;
    
    void Start()
    {
        _playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        
        Debug.Log("GameManager Init");
        _stateManager = new StateManager();
        _stateManager.AddState("Running");
        _stateManager.AddState("Painting");
        _stateManager.AddState("Racing");
        
        this.SetState(0);
    }

    public GameObject GetSpawnPoint() => _spawnPoint;
    
    public void SetState(int ID)
    {
        switch (ID)
        {
            case 0 :
                _playerController.SetMovement(true);
                _spawnPoint = GameObject.Find("Spawn01");
                _playerController.SetSpawnPoint(_spawnPoint);
                foreach (var obs in Stage1Obstacles)
                {
                    obs.SetActive(true);
                }
                break;
            case 1 :
                _playerController.SetMovement(false);
                _spawnPoint = GameObject.Find("Spawn02");
                _playerController.SetSpawnPoint(_spawnPoint);
                _stateManager.SetActiveState(1);
                lerpToSpawn = true;
                Debug.Log("Switching to state " + _stateManager.GetActiveState.StateName);
                break;
                
            default: 
                _stateManager.SetActiveState(0);
                break;
            
        }

    }

    public int GetActiveState() => _stateManager.GetActiveState!=null ? _stateManager.GetActiveState.ID : 0;

    private void Update()
    {
        if (lerpToSpawn)
        {
            lerpValue += Time.deltaTime / 2;
            Vector3 playerPosition = _playerController.transform.position;
            Vector3 newPosition = _spawnPoint.transform.position;
            if (playerPosition == newPosition)
            {
                lerpToSpawn = false;
                return;
            }

            _playerController.SetPlayerLocation(Vector3.Lerp(playerPosition, newPosition, lerpValue));
            _playerController.LerpCamera(lerpValue);
        }
        else
        {
            lerpValue = 0;
        }
    }
}

public class StateManager
{
    private List<State> _states;

    public StateManager()
    {
        Debug.Log("State Manager Init");
        _states = new List<State>();
    }

    public bool IsInitializied() => GetActiveState.isInitializied;
    
    public void AddState(string name)
    {
        State state = new State()
        {
            StateName = name,
            ID = _states.Count,
            isActive = false,
            isInitializied = false
        };
        _states.Add(state);
        Debug.Log("State added with name: " + name);
    }
    
    public List<State> GetStates => _states;

    public void SetActiveState(int id)
    {
        foreach (State state in _states.Where(x=>x.isActive))
        {
            state.isActive = false;
        }

        this.FindByID(id).isActive = true;
    }

    public State FindByID(int ID) => _states.FirstOrDefault(x => x.ID == ID);
    public State GetActiveState => _states.FirstOrDefault(x => x.isActive);



}


public class State
{
    public int ID { get; set; }
    public string StateName { get; set; }
    public bool isActive { get; set; }

    public bool isInitializied { get; set; }

}