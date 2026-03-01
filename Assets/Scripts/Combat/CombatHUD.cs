using TMPro;
using UnityEngine;

namespace Combat
{
    public class CombatHUD: MonoBehaviour
    {
        [SerializeField] private TMP_Text _bossHealthText;
        public void UpdateBossHealth(int currentHealth)
        {
            _bossHealthText.SetText("Health: "+currentHealth);
        }
    }
}