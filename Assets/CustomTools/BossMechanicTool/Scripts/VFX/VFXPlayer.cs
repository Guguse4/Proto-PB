using System.Collections.Generic;
using UnityEngine;

namespace BossMechanicTool.VFX
{
    public class VFXPlayer: MonoBehaviour
    {
        private readonly Dictionary<string, Queue<GameObject>> _idToVisuals = new();

        public void ShowTelegraphPattern(string id, Pattern pattern, Vector3 origin, GameObject attachedObject)
        {
            Transform parent = attachedObject != null ? attachedObject.transform : transform;
            Vector3 position = attachedObject != null ? attachedObject.transform.position : origin;
            
            var visual = Instantiate(
                pattern.TelegraphPrefab,
                position + pattern.SourceRelativePosition,
                Quaternion.identity,
                parent
            );

            visual.transform.localScale = pattern.GetActionSize();
            
            RegisterVisual(id, visual);
        }

        public void ShowActivationPattern(string id, Pattern pattern, Vector3 origin)
        {
            var visual = Instantiate(
                pattern.ActivationVFX,
                origin + pattern.SourceRelativePosition,
                Quaternion.identity,
                transform
            );

            visual.transform.localScale = pattern.GetActionSize();
            
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