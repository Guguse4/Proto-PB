using System;
using BossMechanicTool.BulletSystem;
using UnityEditor;
using UnityEngine;

namespace BossMechanicTool.Action
{
    [Serializable]
    public class BulletAction: PatternAction
    {
        public Bullet bulletPrefab;
        public EmitterData _emitterData;
        private GameObject emitterGo;
        
        /*
         * Function called when mechanic containing this pattern action need to activate it
         */
        public override void ActivateAction(Vector3 position, Vector3 direction, MechanicObject mechanicObject)
        {
            emitterGo = new GameObject("Emitter");
            Emitter emitter = emitterGo.AddComponent<Emitter>();
            emitter.Init(bulletPrefab, _emitterData);
            emitterGo.transform.parent = mechanicObject.transform;
        }

        public override SizeInformation GetSize()
        {
            SizeInformation sizeInformation = new SizeInformation();
            sizeInformation.Angle = 360;
            sizeInformation.InnerRadius = 0f;
            sizeInformation.Scale = new Vector3(1, 1, 1);
            return sizeInformation;
        }

        public override void StopAction()
        {
            base.StopAction();
            GameObject.DestroyImmediate(emitterGo);
        }
    }
}