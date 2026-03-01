using Combat;

namespace Entity.Boss
{
    public class Boss: Entity
    {
        private CombatHUD _combatHUD;

        private void Start()
        {
            _combatHUD = CombatManager.Instance.GetCombatHUD();
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            _combatHUD.UpdateBossHealthRpc(_currentHealth);
        }

        public void InitMechanicLoader(string mechanicName)
        {
            #if UNITY_EDITOR
            if(_combatHUD == null)
                _combatHUD = FindFirstObjectByType<CombatHUD>();
            #endif
            
            _combatHUD.AttachMechanicLoader(gameObject, mechanicName);
        }

        public void UpdateMechanicLoader(float value)
        {
            #if UNITY_EDITOR
            if(_combatHUD == null)
                _combatHUD = FindFirstObjectByType<CombatHUD>();
            #endif
            
            _combatHUD.UpdateMechanicLoader(value);
        }

        public void HideMechanicLoader()
        {
            _combatHUD.HideMechanicLoader();
        }
    }
}