using System;
using UnityEngine;
using BossMechanicTool.Action;

namespace BossMechanicTool
{
    /*
     * Define all available pattern action types
     */
    public enum ActionType
    {
        DamageArea,
        SpawnMob,
        Bullet
    }
    
    /*
     * Data class to define a pattern
     */
    [Serializable]
    public class Pattern
    {
        // Define the position of the pattern relatively of the mechanic attached source
        [SerializeField]
        private Vector3 _sourceRelativePosition;
        public Vector3 SourceRelativePosition{get{return _sourceRelativePosition;}}
        
        // Define the direction relatively of the mechanic rotation source
        [SerializeField]
        private Vector3 _sourceRelativeDirection;
        public Vector3 SourceRelativeDirection{get{return _sourceRelativeDirection;}}

        // Define the action type of the pattern
        [SerializeField]
        private ActionType _actionType;
        
        // If action type is damage area, show damage area action parameters (see custom editor)
        [SerializeField]
        private DamageAreaAction _damageArea;
        // If action type is spawn, show spawn mob action parameters (see custom editor)
        [SerializeField]
        private SpawnMobAction _spawnMob;
        // If action type is bullet, show bullet action parameters (see custom editor)
        [SerializeField]
        private BulletAction _bullet;

        /*
         * return the correct action parameters according to the current action type
         */
        public PatternAction Action
        {
            get
            {
                switch (_actionType)
                {
                    case ActionType.DamageArea:
                        return _damageArea;
                    case ActionType.SpawnMob:
                        return _spawnMob;
                    case ActionType.Bullet:
                        return _bullet;
                    default:
                        return null;
                }
            }
        }

        // VFX asset to use has telegraph
        [SerializeField]
        private GameObject _telegraphPrefab;
        public GameObject TelegraphPrefab{get{return _telegraphPrefab;}}

        // VFX asset to use has execution
        [SerializeField] 
        private GameObject _activationVFX;
        public GameObject ActivationVFX{get{return _activationVFX;}}

        #region  Setter/Getter
            public void SetRelativePosition(Vector3 relativePosition)
            {
                _sourceRelativePosition = relativePosition;
            }

            public void SetRelativeDirection(Vector3 relativeDirection)
            {
                _sourceRelativeDirection = relativeDirection;
            }
            
            /*
             * Return the global visual size of the current action
             * i.e: for damage area, return the size of the shape
             */
            public Vector3 GetActionSize()
            {
                if(Action != null)
                    return Action.GetSize();
                return Vector3.zero;
            }

        #endregion
    }
}