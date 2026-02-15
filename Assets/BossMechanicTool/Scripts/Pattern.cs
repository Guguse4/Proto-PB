using System;
using UnityEngine;
using BossMechanicTool.Action;

namespace BossMechanicTool
{
    public enum ActionType
    {
        DamageArea,
        SpawnMob,
        Bullet
    }
    
    [Serializable]
    public class Pattern
    {
        [SerializeField]
        private Vector3 _sourceRelativePosition;
        public Vector3 SourceRelativePosition{get{return _sourceRelativePosition;}}
        
        [SerializeField]
        private Vector3 _sourceRelativeDirection;
        public Vector3 SourceRelativeDirection{get{return _sourceRelativeDirection;}}

        [SerializeField]
        private ActionType _actionType;
        
        [SerializeField]
        private DamageAreaAction _damageArea;
        [SerializeField]
        private SpawnMobAction _spawnMob;
        [SerializeField]
        private BulletAction _bullet;

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

        // TODO: Move ?
        
        [SerializeField]
        private GameObject _telegraphPrefab;
        public GameObject TelegraphPrefab{get{return _telegraphPrefab;}}
        
        public void SetRelativePosition(Vector3 relativePosition)
        {
            _sourceRelativePosition = relativePosition;
        }

        public void SetRelativeDirection(Vector3 relativeDirection)
        {
            _sourceRelativeDirection = relativeDirection;
        }
    }
}