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
    public ObjectPool<GameObject> pool;
    
    void Awake()
    {
        pool = new ObjectPool<GameObject>(
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
        customHandler.SetPool(this);
        NetworkManager.PrefabHandler.AddHandler(projectilePrefab, customHandler);
    }

    private GameObject CreateProjectile()
    {
        GameObject projectileGO = Instantiate(projectilePrefab);
        projectileGO.gameObject.SetActive(false);
        projectileGO.GetComponent<Projectile>().ObjectPool = this;
        return projectileGO;
    }

    private void OnGetFromPool(GameObject pooledObject)
    {
        
    }

    private void OnReleaseToPool(GameObject pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
        pooledObject.GetComponent<NetworkObject>().Despawn(true);
    }

    private void OnDestroyPooledObject(GameObject pooledObject)
    {
        Destroy(pooledObject);
    }
}
