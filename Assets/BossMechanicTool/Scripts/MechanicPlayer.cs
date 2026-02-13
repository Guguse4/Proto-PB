using BossMechanicTool.Telegraph;
using UnityEngine;

namespace BossMechanicTool
{
    [RequireComponent(typeof(TelegraphRenderer))]
    public class MechanicPlayer: MonoBehaviour
    {
        private TelegraphRenderer _telegraphRenderer;
        
        private void Start()
        {
            _telegraphRenderer = GetComponent<TelegraphRenderer>();
        }

        public void ShowMechanicTelegraph(Mechanic mechanic)
        {
            Debug.Log("Show Mechanic Telegraph");
            if(_telegraphRenderer == null)
                _telegraphRenderer = GetComponent<TelegraphRenderer>();
            
            _telegraphRenderer.Show(mechanic.patterns, transform.position);
        }

        public void HideMechanicTelegraph(Mechanic mechanic)
        {
            Debug.Log("Hide Mechanic Telegraph");
            if(_telegraphRenderer == null)
                _telegraphRenderer = GetComponent<TelegraphRenderer>();
            
            _telegraphRenderer.Hide();
        }

        public void ActivateMechanic(Mechanic mechanic)
        {
            Debug.Log("Activate Mechanic");
        }
    }
}