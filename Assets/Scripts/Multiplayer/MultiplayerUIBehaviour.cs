using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerUIBehaviour : NetworkBehaviour
{
    public void OnPlayCallback()
    {
        if (IsServer)
        {
            Debug.Log("Play !");
            NetworkManager.Singleton.SceneManager.LoadScene(
                SceneUtility.GetScenePathByBuildIndex(1),
                LoadSceneMode.Single
            );
        }
    }
}
