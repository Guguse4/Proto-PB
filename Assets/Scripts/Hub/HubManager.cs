using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hub
{
    public class HubManager : NetworkBehaviour
    {
        public static HubManager Instance { get; set; }
        [SerializeField] private BossSelectorData _bossSelectorData;
        
        private void Awake()
        {
            Instance = this;
        }
        
        public void BossStart(int sceneIndex)
        {
            if (sceneIndex < 0 || sceneIndex >= _bossSelectorData.SceneList.Count)
            {
                return;
            }
            
            Debug.Log("Select boss");
            NetworkManager.Singleton.SceneManager.LoadScene(
                _bossSelectorData.SceneList[sceneIndex].name,
                LoadSceneMode.Single
            );
        }
    }
}