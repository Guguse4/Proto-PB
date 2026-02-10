using System.Collections.Generic;
using BulletHell.Emitter;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace BulletHellTool.Telegraph
{
    public class TelegraphRenderer: MonoBehaviour
    {
        public GameObject decalPrefab;

        private readonly List<GameObject> _decals = new();

        public void Show(IEnumerable<SpawnData> spawns)
        {
            Hide();

            foreach (var spawn in spawns)
            {
                var decal = Instantiate(
                    decalPrefab,
                    spawn.position,
                    Quaternion.LookRotation(spawn.direction, Vector3.left),
                    transform
                );

                _decals.Add(decal);
            }
        }

        public void Hide()
        {
            foreach (var decal in _decals)
                Destroy(decal.gameObject);

            _decals.Clear();
        }
    }
}