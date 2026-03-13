using System;
using BossMechanicTool.BulletSystem;
using UnityEngine;

namespace BossMechanicTool.Action
{
    [Serializable]
    public class BulletAction: PatternAction
    {
        public Bullet bulletPrefab;
        public EmitterData _emitterData;
        
        /*
         * Function called when mechanic containing this pattern action need to activate it
         */
        public override void ActivateAction(Vector3 position, Vector3 direction)
        {
            GameObject emitterGO = new GameObject("Emitter");
            Emitter emitter = emitterGO.AddComponent<Emitter>();
            emitter.Init(bulletPrefab, _emitterData);
        }

        public override Vector3 GetSize()
        {
            return new Vector3(1, 1, 1);
        }
    }
}