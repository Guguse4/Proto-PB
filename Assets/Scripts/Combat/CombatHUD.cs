using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Combat
{
    public class CombatHUD: NetworkBehaviour
    {
        [SerializeField] private TMP_Text _bossHealthText;
        
        [Rpc(SendTo.Everyone)]
        public void UpdateBossHealthRpc(int currentHealth)
        {
            _bossHealthText.SetText("Health: "+currentHealth);
        }
    }
}