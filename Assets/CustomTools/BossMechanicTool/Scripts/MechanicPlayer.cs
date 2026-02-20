using System.Collections.Generic;
using BossMechanicTool.Telegraph;
using UnityEngine;

namespace BossMechanicTool
{
    public struct MechanicPlayerData
    {
        public string id;
        public Mechanic data;
        public Vector3 origin;
        public Vector3 direction;
        public bool isTelegraph;
    }
    
    [RequireComponent(typeof(TelegraphRenderer))]
    public class MechanicPlayer: MonoBehaviour
    {
        // Tool to draw telegraph
        private TelegraphRenderer _telegraphRenderer;
        
        private Dictionary<string, MechanicPlayerData> _mechanicPlayerData = new Dictionary<string, MechanicPlayerData>();
        
        private void Start()
        {
            _telegraphRenderer = GetComponent<TelegraphRenderer>();
        }

        public void ShowMechanicTelegraph(Mechanic mechanic, string id)
        {
            if(_telegraphRenderer == null)
                _telegraphRenderer = GetComponent<TelegraphRenderer>();

            GenerateDataForMechanic(mechanic, id);
            MechanicPlayerData mechanicPlayerData = _mechanicPlayerData[id];
            mechanicPlayerData.isTelegraph = true;
            
            _telegraphRenderer.PlayMechanic(mechanicPlayerData);
        }

        public void ActivateMechanic(Mechanic mechanic, string id)
        {
            foreach (Pattern pattern in mechanic.patterns)
            {
                if(pattern != null && pattern.Action != null)
                    pattern.Action.ActivateAction(transform.position + pattern.SourceRelativePosition, pattern.SourceRelativeDirection);
            }
            
            GenerateDataForMechanic(mechanic, id);
            MechanicPlayerData mechanicPlayerData = _mechanicPlayerData[id];
            mechanicPlayerData.isTelegraph = false;
            
            _telegraphRenderer.PlayMechanic(mechanicPlayerData);
        }
        
        public void HideMechanicTelegraph(string id)
        {
            if(_telegraphRenderer == null)
                _telegraphRenderer = GetComponent<TelegraphRenderer>();
            
            if(_mechanicPlayerData.ContainsKey(id))
                _telegraphRenderer.Hide(_mechanicPlayerData[id]);
            
            _mechanicPlayerData.Remove(id);
        }

        private void GenerateDataForMechanic(Mechanic mechanic, string id)
        {
            if (_mechanicPlayerData.ContainsKey(id) == false)
            {
                MechanicPlayerData mechanicPlayerData =  new MechanicPlayerData();
                mechanicPlayerData.id = id;
                mechanicPlayerData.data = mechanic;
                mechanicPlayerData.origin = transform.position;
                mechanicPlayerData.direction = transform.forward;
                _mechanicPlayerData.Add(id, mechanicPlayerData);
            }
        }
    }
}