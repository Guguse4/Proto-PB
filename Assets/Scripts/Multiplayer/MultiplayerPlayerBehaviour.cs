using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Multiplayer
{
    public class MultiplayerPlayerBehaviour: NetworkBehaviour
    {
        /*
        [SerializeField] PlayerInput _playerInput;
        [SerializeField] AttackController _attackController;
        [SerializeField] PlayerController _playerController;

        private void Awake()
        {
            _playerInput.enabled = false;
            _attackController.enabled = false;
            _playerController.enabled = false;
        }

        private void Start()
        {
            base.OnNetworkSpawn();
            
            if (IsOwner)
            {
                _playerInput.enabled = true;
                _attackController.enabled = true;
                _playerController.enabled = true;
            }
        }
        */
    }
}