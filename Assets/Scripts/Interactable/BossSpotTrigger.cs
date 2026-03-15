using System;
using Hub;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace Interactable
{
    public class BossSpotTrigger : NetworkBehaviour, IInteractable
    {
        private Vector2 _position = new Vector2();
        private Vector2 _popUpOffset = new Vector2(0, 50f);

        [SerializeField] private int _bossSceneIndex;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerController>(out PlayerController playerController))
            {
                _position = Camera.main.WorldToScreenPoint(transform.position);
                HubUIManager.Instance.ShowInteractPopUp(_position + _popUpOffset);
                playerController.SetNearestInteractable(this);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<PlayerController>(out PlayerController playerController))
            {
                HubUIManager.Instance.HideInteractPopUp(_position + _popUpOffset);
                playerController.SetNearestInteractable(null);
            }
        }

        public void OnInteract()
        {
            HubUIManager.Instance.ShowBossInfoWindow(_bossSceneIndex);
        }
    }
}

