using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace BossMechanicTool.VFX
{
    [RequireComponent(typeof(DecalProjector))]
    public class AutomaticDecalSize: MonoBehaviour
    {
        private void Start()
        {
            Vector3 size =  transform.localScale;
            DecalProjector projector = GetComponent<DecalProjector>();
            
            projector.uvScale = new Vector2(projector.uvScale.x *  size.x, projector.uvScale.y *  size.y);
        }
    }
}