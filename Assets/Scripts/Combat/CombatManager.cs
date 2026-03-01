using Combat;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class CombatManager : NetworkBehaviour
{
    [Tooltip("The prefab of the player upon lobby connection")]
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private CombatHUD _combatHUD;
    [SerializeField] private GameSettings.GameSettings _settings;
    
    public static CombatManager Instance { get; set; }
    
    private GameObject _playerPrefabInstance;
    private bool _loaded = false;

    private void Awake()
    {
        Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += (sceneName, loadMode, a, b) =>
        {
            if (_loaded)
                return;
            
            _loaded = true;
            if (IsServer)
            {
                SpawnAllPlayerInScene();
            }
        };
    }

    public void SpawnAllPlayerInScene()
    {
        foreach(ulong cliendId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            SpawnPlayer(cliendId);
        }
    }

    public void SpawnPlayer(ulong cliendId)
    {
        Vector3 position = transform.position;
        position.x = Random.Range(-10, 10);
        GameObject player = Instantiate(_playerPrefab, position, Quaternion.identity);
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(cliendId);
    }
    
    public GameSettings.GameSettings GetSettings()
    {
        return _settings;
    }

    public CombatHUD GetCombatHUD()
    {
        return _combatHUD;
    }
}
