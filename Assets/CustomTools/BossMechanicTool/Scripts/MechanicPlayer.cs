using BossMechanicTool.Telegraph;
using UnityEngine;

namespace BossMechanicTool
{
    [RequireComponent(typeof(TelegraphRenderer))]
    public class MechanicPlayer: MonoBehaviour
    {
        // Tool to draw telegraph
        private TelegraphRenderer _telegraphRenderer;
        
        private void Start()
        {
            _telegraphRenderer = GetComponent<TelegraphRenderer>();
        }

        public void ShowMechanicTelegraph(Mechanic mechanic)
        {
            if(_telegraphRenderer == null)
                _telegraphRenderer = GetComponent<TelegraphRenderer>();
            
            _telegraphRenderer.Show(mechanic.patterns, transform.position);
        }

        public void HideMechanicTelegraph(Mechanic mechanic)
        {
            if(_telegraphRenderer == null)
                _telegraphRenderer = GetComponent<TelegraphRenderer>();
            
            _telegraphRenderer.Hide();
        }

        public void ActivateMechanic(Mechanic mechanic)
        {
            foreach (Pattern pattern in mechanic.patterns)
            {
                if(pattern != null && pattern.Action != null)
                    pattern.Action.ActivateAction(transform.position + pattern.SourceRelativePosition, pattern.SourceRelativeDirection);
            }
        }
    }
}