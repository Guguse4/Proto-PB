using Unity.Netcode;
using UnityEngine;

public class CombatManager : NetworkBehaviour
{
    [Tooltip("The prefab of the player upon lobby connection")]
    [SerializeField] private GameObject _playerPrefab;
        
    private GameObject _playerPrefabInstance;
    
    void Start()
    {
        _playerPrefabInstance = Instantiate(_playerPrefab, transform.position, Quaternion.identity);
        _playerPrefabInstance.GetComponent<NetworkObject>().Spawn();
    }
}
