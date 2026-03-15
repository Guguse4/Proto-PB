using Unity.Netcode;
using UnityEngine;

namespace Hub
{
    public class HubManager : NetworkBehaviour
    {
        [SerializeField] private GameObject _playerPrefab;
        
        public static HubManager Instance { get; set; }
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
    }
}