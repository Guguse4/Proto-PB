using System;
using Hub;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class BossSpotTrigger : NetworkBehaviour
{
    private Vector2 _position = new Vector2();
    private Vector2 _popUpOffset = new Vector2(0, 50f);
    private void OnTriggerEnter(Collider other)
    {
        _position = Camera.main.WorldToScreenPoint(transform.position);
        HubUIManager.Instance.ShowInteractPopUp(_position + _popUpOffset);
        Debug.Log("Position : " + _position);
    }

    private void OnTriggerExit(Collider other)
    {
        HubUIManager.Instance.HideInteractPopUp(_position + _popUpOffset);
        Debug.Log("Exit");
    }
}
