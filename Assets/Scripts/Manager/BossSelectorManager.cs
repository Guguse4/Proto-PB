using System;
using TMPro;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossSelectorManager : NetworkBehaviour
{
    [SerializeField]
    private BossSelectorData _data;
    [SerializeField]
    private GameObject _buttonContent;
    [SerializeField]
    private Button _buttonPrefab;

    private void Start()
    {
        foreach(SceneAsset scene in _data.SceneList)
        {
            Button button = Instantiate(_buttonPrefab, _buttonContent.transform);
            button.GetComponentInChildren<TMP_Text>().text = scene.name;
            
            if (!IsServer)
            {
                button.interactable = false;
                return;
            }
            
            button.onClick.AddListener(() => OnButtonClick(scene));
        }
    }

    private void OnButtonClick(SceneAsset scene)
    {
        Debug.Log("Select boss");
        NetworkManager.Singleton.SceneManager.LoadScene(
            scene.name,
            LoadSceneMode.Single
        );
    }
}
