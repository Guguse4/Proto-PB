using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Manager
{
    public class GameManager : NetworkBehaviour
    {
        [SerializeField] private GameObject _playerPrefab;
        public static GameManager Instance { get; set; }
        private bool _loaded = false;

        private List<GameObject> _playersList;
        
        private void Awake()
        {
            Instance = this;
            _playersList = new List<GameObject>();
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
            _playersList.Add(player);
        }

        public void RefreshPlayer()
        {
            for (int i = 0; i < _playersList.Count; i++)
            {
                _playersList[i].GetComponent<PlayerController>().currentCamera =  Camera.main;
            }
        }
    }
}