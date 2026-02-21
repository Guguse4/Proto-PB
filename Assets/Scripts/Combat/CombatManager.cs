using Unity.Netcode;
using UnityEngine;

public class CombatManager : NetworkBehaviour
{
    [Tooltip("The prefab of the player upon lobby connection")]
    [SerializeField] private GameObject _playerPrefab;
        
    private GameObject _playerPrefabInstance;
    private bool _loaded = false;
    
    void Start()
    {
        _playerPrefabInstance = Instantiate(_playerPrefab, transform.position, Quaternion.identity);
        _playerPrefabInstance.GetComponent<NetworkObject>().Spawn();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += (sceneName, loadMode, a, b) =>
        {
            if (_loaded)
                return;

            _loaded = true;
            if (IsSessionOwner)
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
        GameObject player = Instantiate(_playerPrefab, transform.position, Quaternion.identity);
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(cliendId);
    }
}
