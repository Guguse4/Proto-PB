using Entity.Boss;
using Entity.Player;
using Unity.Netcode;
using UnityEngine;

namespace Combat
{
    public class GameManager : NetworkBehaviour
    {
        [SerializeField] private CombatHUD _combatHUD;
        [SerializeField] private GameSettings.GameSettings _settings;
    
        public static GameManager Instance { get; set; }
        private Player _player;
        private Boss _boss;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            
            _boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
            _boss.OnTakeDamage.AddListener(OnBossTakeDamage);
        }

        public GameSettings.GameSettings GetSettings()
        {
            return _settings;
        }

        private void OnBossTakeDamage(int currentHealth)
        {
            _combatHUD.UpdateBossHealthRpc(currentHealth);
        }
    }
}