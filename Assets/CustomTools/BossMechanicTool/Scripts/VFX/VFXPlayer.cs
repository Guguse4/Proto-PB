using System.Collections.Generic;
using UnityEngine;

namespace BossMechanicTool.VFX
{
    /*
     * Class used to manage vfx instantiation and destroy
     */
    public class VFXPlayer: MonoBehaviour
    {
        // Save vfx pool according to mechanic id
        private readonly Dictionary<string, Queue<GameObject>> _idToVisuals = new();

        /*
         * Used to instantiate a copy of vfxRef at the given position, rotation and size
         * Attach it at the given parent
         */
        public void ShowVfx(string id, GameObject vfxRef, Vector3 position, Quaternion rotation, Vector3 size, Transform parent)
        {
            // Instantiate vfx
            GameObject visual = Instantiate(
                vfxRef,
                position,
                rotation,
                parent != null ? parent : transform
            );
            
            // Scale it
            visual.transform.localScale = size;
            // Register it
            RegisterVisual(id, visual);
        }

        /*
         * Used to destroy the first element of the vfx pool for the given mechanic id
         */
        public void HidePattern(string id)
        {
            // Check if pool exist
            if (_idToVisuals.ContainsKey(id) == false)
                return;
            
            // Get the pool
            Queue<GameObject> visuals = _idToVisuals[id];
            
            // Get the first element
            GameObject visual = visuals.Dequeue();
            
            // Destroy element if valid
            if(visual != null)
                DestroyImmediate(visual);
            
            // Remove pool of dictionary if empty
            if (_idToVisuals[id].Count == 0)
            {
                _idToVisuals.Remove(id);
            }
        }
        
        /*
         * Used to register vfx in the correct pool
         */
        private void RegisterVisual(string id, GameObject visual)
        {
            // if dictionary already contain a pool for the given mechanic id, add vfx in it
            if (_idToVisuals.ContainsKey(id))
            {
                _idToVisuals[id].Enqueue(visual);
            }
            else
            {
                // Create pool and add vfx in it
                Queue<GameObject> visuals = new();
                visuals.Enqueue(visual);
                _idToVisuals.Add(id, visuals);
            }
        }

        /*
         * Used to return the number of element in the pool for the given mechanic id
         */
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