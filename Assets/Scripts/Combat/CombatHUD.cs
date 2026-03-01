using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Combat
{
    public class CombatHUD: NetworkBehaviour
    {
        // Boss health
        [SerializeField] private TMP_Text _bossHealthText;
        // Mechanic
        private GameObject _attachedMechanicLoader;
        [SerializeField] private TMP_Text _mechanicText;
        [SerializeField] private Slider _bossMechanicLoader;

        private Camera _camera;

        private void Start()
        {
            _camera = FindFirstObjectByType<Camera>();
        }

        [Rpc(SendTo.Everyone)]
        public void UpdateBossHealthRpc(int currentHealth)
        {
            _bossHealthText.SetText("Health: "+currentHealth);
        }

        public void AttachMechanicLoader(GameObject attachedObject, string mechanicName)
        {
            #if UNITY_EDITOR
            if (_camera == null)
                _camera = FindFirstObjectByType<Camera>();
            #endif
            _attachedMechanicLoader = attachedObject;
            _bossMechanicLoader.gameObject.SetActive(true);
            _mechanicText.SetText(mechanicName);
        }

        public void UpdateMechanicLoader(float value)
        {
            _bossMechanicLoader.value = value;
        }

        public void HideMechanicLoader()
        {
            _bossMechanicLoader.gameObject.SetActive(false);
        }

        private void Update()
        {
            if(_attachedMechanicLoader != null)
                _bossMechanicLoader.transform.position = _camera.WorldToScreenPoint(_attachedMechanicLoader.transform.position);
        }
    }
}