using System;
using Combat;
using Entity.Boss;
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
    private Boss _boss;
    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += (sceneName, loadMode, a, b) =>
        {
            if (_loaded)
                return;

            Instance = this;
            _loaded = true;
            if (IsServer)
            {
                SpawnAllPlayerInScene();
                _boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
                _boss.OnTakeDamage.AddListener(OnBossTakeDamage);
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

    private void OnBossTakeDamage(int currentHealth)
    {
        _combatHUD.UpdateBossHealthRpc(currentHealth);
    }
    
    public GameSettings.GameSettings GetSettings()
    {
        return _settings;
    }
}
