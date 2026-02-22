using System.Collections.Generic;
using BossMechanicTool.Timeline;
using UnityEngine;

namespace BossMechanicTool.VFX
{
    public class VFXPlayer: MonoBehaviour
    {
        private readonly Dictionary<string, Queue<GameObject>> _idToVisuals = new();

        public void ShowVfx(string id, GameObject vfxRef, Vector3 position, Quaternion rotation, Vector3 size, Transform parent)
        {
            GameObject visual = Instantiate(
                vfxRef,
                position,
                rotation,
                parent != null ? parent : transform
            );

            visual.transform.localScale = size;
            
            RegisterVisual(id, visual);
        }

        public void HidePattern(string id)
        {
            if (_idToVisuals.ContainsKey(id) == false)
                return;
            
            Queue<GameObject> visuals = _idToVisuals[id];
            GameObject visual = visuals.Dequeue();
            
            if(visual != null)
                DestroyImmediate(visual);
            
            if (_idToVisuals[id].Count == 0)
            {
                _idToVisuals.Remove(id);
            }
        }
        
        private void RegisterVisual(string id, GameObject visual)
        {
            if (_idToVisuals.ContainsKey(id))
            {
                _idToVisuals[id].Enqueue(visual);
            }
            else
            {
                Queue<GameObject> visuals = new();
                visuals.Enqueue(visual);
                _idToVisuals.Add(id, visuals);
            }
        }

        public int GetVFXNumber(string id)
        {
            if (_idToVisuals.ContainsKey(id))
            {
                return _idToVisuals[id].Count;
            }

            return 0;
        }
    }
}