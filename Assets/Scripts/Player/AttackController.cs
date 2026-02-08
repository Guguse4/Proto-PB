using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.InputSystem;
using System;

public class AttackController : MonoBehaviour
{
    [Tooltip("Prefab to shoot")]
    [SerializeField] private Projectile projectilePrefab;

    [SerializeField] private ScriptableObject projectileDataSO;

    private ObjectPool<Projectile> pool;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        pool = new ObjectPool<Projectile>(
            createFunc: CreateProjectile,
            actionOnGet: OnGetFromPool,
            actionOnRelease: OnReleaseToPool,
            actionOnDestroy: OnDestroyPooledObject,
            collectionCheck: true,
            defaultCapacity: 20,
            maxSize: 100
            );
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    private Projectile CreateProjectile()
    {
        Projectile projectileGO = Instantiate(projectilePrefab);
        projectileGO.ObjectPool = pool;
        return projectileGO;
    }

    private void OnGetFromPool(Projectile pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
    }

    private void OnReleaseToPool(Projectile pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }

    private void OnDestroyPooledObject(Projectile pooledObject)
    {
        Destroy(pooledObject);
    }

    private System.Collections.IEnumerator ReturnAfter(GameObject gameObject, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        // Give it back to the pool.
        pool.Release(gameObject);
    }
}
