using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.InputSystem;
using System;
using Combat;
using Unity.Netcode;

public class BulletPool : NetworkBehaviour
{
    [Tooltip("Prefab to shoot")]
    [SerializeField] private GameObject projectilePrefab;

    public ObjectPool<Projectile> pool;
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

    private void Start()
    {
        var customHandler = new NetworkBulletHandler();
        NetworkManager.PrefabHandler.AddHandler(projectilePrefab, customHandler);
    }

    private Projectile CreateProjectile()
    {
        GameObject projectileGO = Instantiate(projectilePrefab);
        projectileGO.GetComponent<NetworkObject>().Spawn();
        projectileGO.gameObject.SetActive(false);
        projectileGO.GetComponent<Projectile>().ObjectPool = this;
        return projectileGO.GetComponent<Projectile>();
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
}
