using System;
using UnityEngine;
using BossMechanicTool.Action;

namespace BossMechanicTool
{
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
        private PatternAction _action;
        public PatternAction Action{get{return _action;}}
        
        // TODO: Move ?
        [SerializeField]
        private GameObject _telegraphPrefab;
        public GameObject TelegraphPrefab{get{return _telegraphPrefab;}}
    }
}