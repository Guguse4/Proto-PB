using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

namespace Hub
{
    public class HubUIManager : MonoBehaviour
    {
        public static HubUIManager Instance { get; set; }
        
        [SerializeField]
        private GameObject _interactPopUp;
        
        [SerializeField]
        private GameObject _bossInfoWindow;
        
        private Dictionary<Vector2, GameObject> _popUps = new Dictionary<Vector2, GameObject>();
        private int _currentSceneIndex = -1;
        
        private void Awake()
        {
            Instance = this;
        }

        public void ShowInteractPopUp(Vector2 position)
        {
            GameObject popUp = Instantiate(_interactPopUp, transform, false);
            RectTransform rectTransform = popUp.GetComponent<RectTransform>();
            rectTransform.position = position;
            _popUps.Add(position, popUp);
        }

        public void HideInteractPopUp(Vector2 position)
        {
            if (_popUps.ContainsKey(position))
            {
                Destroy(_popUps[position]);
                _popUps.Remove(position);
            }
        }

        public void ShowBossInfoWindow(int sceneIndex)
        {
            _currentSceneIndex = sceneIndex;
            _bossInfoWindow.SetActive(true);
        }

        public void HideBossInfoWindow()
        {
            _currentSceneIndex = -1;
            _bossInfoWindow.SetActive(false);
        }

        public void OnBossStartClick()
        {
            HubManager.Instance.BossStart(_currentSceneIndex);
        }
    }
}