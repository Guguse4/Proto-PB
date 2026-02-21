using System.Collections.Generic;
using UnityEngine;

namespace BossMechanicTool.VFX
{
    public class VFXPlayer: MonoBehaviour
    {
        private readonly Dictionary<string, Queue<GameObject>> _idToVisuals = new();

        public void ShowTelegraphPattern(string id, Pattern pattern, Vector3 origin, GameObject attachedObject)
        {
            var visual = Instantiate(
                pattern.TelegraphPrefab,
                origin + pattern.SourceRelativePosition,
                Quaternion.identity,
                attachedObject != null ? attachedObject.transform : transform
            );

            visual.transform.localScale = pattern.GetActionSize();
            
            RegisterVisual(id, visual);
        }

        public void ShowActivationPattern(string id, Pattern pattern, Vector3 origin, GameObject attachedObject)
        {
            var visual = Instantiate(
                pattern.ActivationVFX,
                origin + pattern.SourceRelativePosition,
                Quaternion.identity,
                attachedObject != null ? attachedObject.transform : transform
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
    }
}