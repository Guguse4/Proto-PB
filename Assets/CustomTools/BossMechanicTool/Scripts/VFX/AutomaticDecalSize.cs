using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace BossMechanicTool.VFX
{
    /*
     * Used by decal prefab to automatically adapt tilling with the size
     */
    [RequireComponent(typeof(DecalProjector)), ExecuteInEditMode]
    public class AutomaticDecalSize: MonoBehaviour
    {
        private void Start()
        {
            // Find parent local scale
            if (transform.parent == null)
            {
                Debug.LogWarning("AutomaticDecalSize: parent object is null");
                return;
            }
            Vector3 size =  transform.parent.localScale;
            
            
            DecalProjector projector = GetComponent<DecalProjector>();
            projector.uvScale = new Vector2(projector.uvScale.x *  size.x, projector.uvScale.y *  size.z);
        }
    }
}