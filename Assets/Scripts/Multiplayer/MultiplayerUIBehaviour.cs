using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerUIBehaviour : NetworkBehaviour
{
    public void OnPlayCallback()
    {
        if (IsServer)
        {
            Debug.Log("Select boss");
            NetworkManager.Singleton.SceneManager.LoadScene(
                SceneUtility.GetScenePathByBuildIndex(1),
                LoadSceneMode.Single
            );
        }
    }
}
