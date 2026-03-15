using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Hub
{
    public class HubUIManager : NetworkBehaviour
    {
        public static HubUIManager Instance { get; set; }
        
        [SerializeField]
        private GameObject _interactPopUp;
        
        private Dictionary<Vector2, GameObject> _popUps = new Dictionary<Vector2, GameObject>();
        
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
    }
}